using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Lms_backend.IntegrationTests.Infrastructure;

// Shared across every controller's integration tests via IntegrationTestCollection - one
// Postgres container and one host for the whole run, reset back to a known seeded state
// between individual tests instead of paying container/host startup cost per test.
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("lms_integration_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private NpgsqlConnection _connection = null!;
    private Respawner _respawner = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:postgres", _dbContainer.GetConnectionString());
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Migrations must exist before the host is first built below, since building it runs
        // Program.cs's RoleSeeder/UserSeeder against the database as part of startup.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;
        await using (var migrationContext = new AppDbContext(options))
        {
            await migrationContext.Database.MigrateAsync();
        }

        // Touching Services forces WebApplicationFactory to build the host now rather than
        // lazily on first client request, so seeding happens before any test runs.
        using (Services.CreateScope())
        {
        }

        _connection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["identity"],
        });
    }

    // Wipes all app data and re-runs the app's own seeders, so every test starts from the same
    // baseline (roles + the example users from UserSeeder) regardless of what earlier tests did.
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);

        using (var scope = Services.CreateScope())
        {
            // UserSeeder only patches these users if they already exist (normally guaranteed by
            // the SeedExampleData migration) - recreate the bare rows since Respawn wiped them.
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            foreach (var (id, userName, _) in TestUsers.Seeded)
            {
                if (await userManager.FindByIdAsync(id.ToString()) is null)
                {
                    await userManager.CreateAsync(new ApplicationUser { Id = id, UserName = userName });
                }
            }
        }

        await RoleSeeder.SeedAsync(Services);
        await UserSeeder.SeedAsync(Services);
    }

    public HttpClient CreateAuthClient() =>
        CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _connection.DisposeAsync();
        await base.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}
