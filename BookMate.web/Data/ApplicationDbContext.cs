using BookMate.web.Core.Models;
using BookMate.web.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options):base(Options) 
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalBook> RentalBooks { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BookCategoryConfiguration());    
            builder.ApplyConfiguration(new RentalBookConfiguration());

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(builder);
        }


    }
}
