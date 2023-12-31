﻿using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using UserModule.Data;
using UserModule.Data.Models;

namespace UserModule.Extensions;

public static class UserAppExtensions
{
    private const string InMemoryProviderName = "Microsoft.EntityFrameworkCore.InMemory";

    public static async Task SeedDatabase(this WebApplication app)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("UserModule: database seeding starting.");
        await SeedDatabaseInternal(app);

        stopwatch.Stop();
        var elapsedTime = stopwatch.Elapsed;
        Console.WriteLine($"UserModule: database seeding completed in {elapsedTime.TotalMilliseconds}ms.");
    }

    private static async Task SeedDatabaseInternal(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        await context.Database.EnsureDeletedAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (IsInMemoryDatabase(context))
        {
            await context.Database.EnsureCreatedAsync();
            await SeedRoles(roleManager);
        }
        else
        {
            await context.Database.MigrateAsync();
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        await context.SaveChangesAsync();
    }

    private static bool IsInMemoryDatabase(DbContext context)
    {
        return context.Database.ProviderName == InMemoryProviderName;
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { UserRoles.User.ToString(), UserRoles.Admin.ToString() };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }

        Console.WriteLine("UserModule: role seeding complete.");
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        var adminUser = CreateUser("Owner", "xxxx@example.com", UserRoles.Admin.ToString(), "c0bdebd1-f275-4722-aa54-ca4524e4b998");
        var normalUser = CreateUser("User", "user@example.com", UserRoles.User.ToString(), "337fa2c2-09bc-44e4-ac44-3d5641619829");

        await AddUser(userManager, adminUser, "secretPassword12@");
        await AddUser(userManager, normalUser, "secretPassword12@");

        Console.WriteLine("UserModule: User seeding complete.");
    }

    private static ApplicationUser CreateUser(string userName, string email, string role, string userId)
    {
        return new ApplicationUser
        {
            Id = userId,
            FirstName = userName,
            LastName = "Ihenacho",
            Email = email,
            NormalizedEmail = email.ToUpper(),
            UserName = userName,
            NormalizedUserName = userName.ToUpper(),
            PhoneNumber = $"+1{userName.Length}1".PadLeft(12, '1'),
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            Role = role
        };
    }

    private static async Task AddUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var hashedPassword = passwordHasher.HashPassword(user, password);
        user.PasswordHash = hashedPassword;

        await userManager.CreateAsync(user);
        await userManager.AddToRoleAsync(user, user.Role);
    }
}