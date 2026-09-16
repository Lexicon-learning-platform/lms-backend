using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Constants;

namespace Lms_backend.Infrastructure;

public static class UserSeeder
{
    public const string DefaultPassword = "Password123!";

    private static readonly (Guid Id, string UserName, string Email, string Role)[] ExistingUsers =
    [
        (new Guid("44444444-0000-0000-0000-000000000001"), "alex.nilsson", "alex.nilsson@example.com", Roles.Teacher),
        (new Guid("44444444-0000-0000-0000-000000000002"), "maria.svensson", "maria.svensson@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000003"), "johan.berg", "johan.berg@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000004"), "sara.lindqvist", "sara.lindqvist@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000006"), "emma.karlsson", "emma.karlsson@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000007"), "oskar.lindberg", "oskar.lindberg@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000008"), "lina.hakansson", "lina.hakansson@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000009"), "viktor.astrom", "viktor.astrom@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000010"), "erik.holm", "erik.holm@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000011"), "sofia.bergstrom", "sofia.bergstrom@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000012"), "anders.nystrom", "anders.nystrom@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000013"), "elin.forsberg", "elin.forsberg@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000014"), "fredrik.dahl", "fredrik.dahl@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000015"), "nina.ekstrom", "nina.ekstrom@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000016"), "josefin.lund", "josefin.lund@example.com", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000017"), "martin.oberg", "martin.oberg@example.com", Roles.Student),
    ];

    private static readonly Guid AdminId = new("44444444-0000-0000-0000-000000000005");

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var (id, userName, email, role) in ExistingUsers)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                continue;
            }

            await EnsureCredentialsAsync(userManager, user, userName, email, role);
        }

        var admin = await userManager.FindByIdAsync(AdminId.ToString());
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Id = AdminId,
                UserName = "admin",
                Email = "admin@example.com",
                GivenName = "Ada",
                LastName = "Admin",
            };
            var result = await userManager.CreateAsync(admin);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to seed admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        await EnsureCredentialsAsync(userManager, admin, "admin", "admin@example.com", Roles.Admin);
    }

    private static async Task EnsureCredentialsAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string userName, string email, string role)
    {
        if (!string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase))
        {
            await userManager.SetUserNameAsync(user, userName);
        }

        if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            await userManager.SetEmailAsync(user, email);
        }

        user.EmailConfirmed = true;
        user.Role = role;
        await userManager.UpdateAsync(user);

        if (await userManager.HasPasswordAsync(user))
        {
            await userManager.RemovePasswordAsync(user);
        }

        await userManager.AddPasswordAsync(user, DefaultPassword);

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
