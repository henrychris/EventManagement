using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using UserModule.Data;
using UserModule.Data.Models;

namespace UserModule.Extensions;

public static class UserAppExtensions
{
    public static async Task SeedDatabase(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("Starting UserModule database seeding.");
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await context.SaveChangesAsync();
            Console.WriteLine("UserModule database seeding complete.");
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { UserRoles.User.ToString(), UserRoles.Admin.ToString() };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        Console.WriteLine("UserModule: role seeding complete.");
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        var adminUser = new ApplicationUser
        {
            FirstName = "Henry",
            LastName = "Ihenacho",
            Email = "xxxx@example.com",
            NormalizedEmail = "XXXX@EXAMPLE.COM",
            UserName = "Owner",
            NormalizedUserName = "OWNER",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var normalUser = new ApplicationUser
        {
            FirstName = "User",
            LastName = "Ihenacho",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            UserName = "User",
            NormalizedUserName = "USER",
            PhoneNumber = "+222222222",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };


        await AddUser(userManager, adminUser, UserRoles.Admin.ToString());
        await AddUser(userManager, normalUser, UserRoles.User.ToString());
        Console.WriteLine("UserModule: User seeding complete.");
    }

    private static async Task AddUser(UserManager<ApplicationUser> userManager,
        ApplicationUser user, string role)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (await userManager.FindByEmailAsync(user.Email!) is null)
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, "secretPassword12@");
            user.PasswordHash = hashed;
            user.Role = role;
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}