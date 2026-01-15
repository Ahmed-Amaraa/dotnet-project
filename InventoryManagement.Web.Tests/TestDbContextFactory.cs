using System;
using System.Runtime.CompilerServices;
using InventoryManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Web.Tests;

internal static class TestDbContextFactory
{
    public static ApplicationDbContext Create([CallerMemberName] string? name = null)
    {
        var dbName = name is null ? $"test-{Guid.NewGuid():N}" : $"test-{name}-{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }
}
