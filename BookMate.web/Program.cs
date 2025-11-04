using BookMate.web.Core.Models;
using BookMate.web.Data;
using BookMate.web.Interfaces;
using BookMate.web.Repositories;
using BookMate.web.Seeds;
using BookMate.web.Services;
using BookMate.web.Settings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BookMate.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            
            //Configuer Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            
            //register services
         
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // register Services
            builder.Services.AddTransient<IEmailSender,EmailSender>();
            builder.Services.AddTransient<IEmailBodyBuilder,EmailBodyBuilder>();
            // configer appseting readers
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //seed data
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await DefaultRoles.SeedAsync(roleManger);
            await DefaultUsers.SeedAsync(userManger);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
