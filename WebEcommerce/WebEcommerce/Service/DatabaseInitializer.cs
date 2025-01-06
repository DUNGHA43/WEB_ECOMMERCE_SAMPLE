using Microsoft.AspNetCore.Identity;
using WebEcommerce.Models;

namespace WebEcommerce.Service
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager or roleManager is null => exit");
                return;
            }

            //check if we have the admin rolle or not
            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                Console.WriteLine("Admin role is not defined add will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            exists = await roleManager.RoleExistsAsync("seller");
            if (!exists)
            {
                Console.WriteLine("Seller role is not defined add will be created");
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }

            exists = await roleManager.RoleExistsAsync("client");
            if (!exists)
            {
                Console.WriteLine("Client role is not defined add will be created");
                await roleManager.CreateAsync(new IdentityRole("client"));
            }

            //check if we have at least one admin user or not
            var adminUsers = await userManager.GetUsersInRoleAsync("admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin user already exists => exit");
                return;
            }


            //create the admin user
            var adminUser = new ApplicationUser
            {
                fisrtName = "admin",
                lastName = "admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                address = "Ha Noi, Viet Nam",
                createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string initialPassword = "admin123";


            var result = await userManager.CreateAsync(adminUser, initialPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
                Console.WriteLine("Admin user created successfully! Please update the initial password!");
                Console.WriteLine($"Email: {adminUser.Email}");
                Console.WriteLine($"Initial password: {initialPassword}");
            }
        }
    }
}
