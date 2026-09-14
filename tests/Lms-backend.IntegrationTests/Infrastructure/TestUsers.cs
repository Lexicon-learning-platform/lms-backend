using Lms_backend.Domain.Constants;

namespace Lms_backend.IntegrationTests.Infrastructure;

// UserSeeder only patches the credentials/role of these users if they already exist (in the real
// app they're inserted once by the SeedExampleData migration) - it never creates them from
// scratch. Since resetting the test database wipes them too, IntegrationTestWebAppFactory needs
// this fixed id/username mapping to recreate the bare rows before delegating to UserSeeder.
// Kept in sync with UserSeeder.cs and Lms-backend.Api.http.
public static class TestUsers
{
    public static readonly (Guid Id, string UserName, string Role)[] Seeded =
    [
        (new Guid("44444444-0000-0000-0000-000000000001"), "alex.nilsson", Roles.Teacher),
        (new Guid("44444444-0000-0000-0000-000000000002"), "maria.svensson", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000003"), "johan.berg", Roles.Student),
        (new Guid("44444444-0000-0000-0000-000000000004"), "sara.lindqvist", Roles.Student),
    ];

    public const string StudentUsername = "maria.svensson";
    public const string StudentRole = Roles.Student;
}
