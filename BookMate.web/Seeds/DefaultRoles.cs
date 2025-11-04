using Microsoft.AspNetCore.Identity;

namespace BookMate.web.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync( RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any()) 
            {
                await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Admin });
                await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Archive });
                await roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Reception });
            }
        }
    }
}
