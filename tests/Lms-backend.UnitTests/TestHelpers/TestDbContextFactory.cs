using Lms_backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Lms_backend.UnitTests.TestHelpers;

public static class TestDbContextFactory
{
    public static AppDbContext Create(params IInterceptor[] interceptors) => Create(Guid.NewGuid().ToString(), interceptors);

    public static AppDbContext Create(string databaseName, params IInterceptor[] interceptors)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName);

        if (interceptors.Length > 0) optionsBuilder.AddInterceptors(interceptors);

        return new AppDbContext(optionsBuilder.Options);
    }
}
