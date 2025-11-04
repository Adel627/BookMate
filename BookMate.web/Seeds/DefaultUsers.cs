using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookMate.web.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> manager)
        {
            ApplicationUser admin = new()
            {
                FullName = "admin",
                UserName = "Admin1",
                Email = "admin111@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "01120443011",
                PhoneNumberConfirmed = true,
            };

            
            if (!await manager.Users.AnyAsync()) 
            {
               await manager.CreateAsync(admin,"Admin1@@@");
               await manager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
