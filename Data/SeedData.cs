using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Web.Data;

public static class SeedData
{
    private const string AdminRoleName = "Admin";
    private const string AdminEmail = "admin@local";
    private const string AdminPassword = "Admin#123";

    public static async Task EnsureSeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(AdminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
        }

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var adminUser = await userManager.FindByNameAsync(AdminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, AdminPassword);
            if (!createResult.Succeeded)
            {
                var reasons = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create seed admin user: {reasons}");
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, AdminRoleName))
        {
            await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        }

        if (!await context.Suppliers.AnyAsync())
        {
            var suppliers = new List<Supplier>
            {
                new() { SupplierName = "ElectroSource", ContactNumber = "+33123456789", Email = "contact@electrosource.com", Address = "12 Rue de Paris" },
                new() { SupplierName = "TechnoHub", ContactNumber = "+33198765432", Email = "hello@technohub.io", Address = "45 Avenue des Champs" }
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();

            var products = new List<Product>
            {
                new()
                {
                    ProductName = "Laptop Pro 15",
                    Category = "Ordinateurs",
                    Quantity = 12,
                    UnitPrice = 1499m,
                    ReorderLevel = 5,
                    SupplierId = suppliers[0].SupplierId
                },
                new()
                {
                    ProductName = "Casque Bluetooth",
                    Category = "Audio",
                    Quantity = 40,
                    UnitPrice = 89.99m,
                    ReorderLevel = 15,
                    SupplierId = suppliers[1].SupplierId
                },
                new()
                {
                    ProductName = "Smartphone X",
                    Category = "Mobiles",
                    Quantity = 8,
                    UnitPrice = 999m,
                    ReorderLevel = 10,
                    SupplierId = suppliers[0].SupplierId
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}