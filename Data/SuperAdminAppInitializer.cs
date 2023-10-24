using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MindHeal.FileManagers;
using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;
using System.Data;
using System.Runtime.CompilerServices;

namespace MindHeal.Data
{
    public static class SuperAdminAppInitializer
    {
        public static async Task Seed(this IApplicationBuilder applicationBuilder)
        {

            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Adesanya",
                LastName = "Masturah",
                Email = "masturahadesanya@gmail.com",
                PhoneNumber = "09051643452",
                IsDeleted = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Gender = Gender.Female,
                UserName = "masturahadesanya@gmail.com",
            };

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                await context.Database.MigrateAsync();
                if (!context.Roles.Any())
                {
                    var roler = await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    var userr = await userManager.CreateAsync(user, "@244341Ay");
                    var userMr = await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }

}
