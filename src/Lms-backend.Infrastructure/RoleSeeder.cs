using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Constants;

namespace Lms_backend.Infrastructure;

public static class RoleSeeder
{
  public static async Task SeedAsync(IServiceProvider services)
  {
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    foreach (var role in Roles.All)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new ApplicationRole { Name = role });
      }
    }
  }
}
