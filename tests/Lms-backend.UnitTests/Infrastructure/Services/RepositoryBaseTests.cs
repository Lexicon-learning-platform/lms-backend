using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// RepositoryBase<TEntity> is abstract, so it is exercised here through ActivityRepository,
// which adds no overrides of the members under test.
public class RepositoryBaseTests
{
    private static Activity CreateActivity(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        ModuleId = Guid.NewGuid(),
        ActivityType = ActivityType.Lecture,
        Name = "Test activity",
        Description = "Test description",
        StartTimeOffset = 0,
        DurationMinutes = 30,
    };

    [Fact]
    public async Task AddAsync_AddsEntity_SoItExistsAfterSaveChanges()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);
        var activity = CreateActivity();

        await repository.AddAsync(activity, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.True(await repository.ExistsAsync(activity.Id, CancellationToken.None));
    }

    [Fact]
    public void Delete_RemovesTrackedEntity_FromSet()
    {
        using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);
        var activity = CreateActivity();
        context.Activities.Add(activity);
        context.SaveChanges();

        repository.Delete(activity);
        context.SaveChanges();

        Assert.False(context.Activities.Any(a => a.Id == activity.Id));
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenEntityExists()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);

        Assert.True(await repository.ExistsAsync(activity.Id, CancellationToken.None));
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenEntityDoesNotExist()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);

        Assert.False(await repository.ExistsAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task GetMissingIdsAsync_ReturnsOnlyIdsThatDoNotExist()
    {
        await using var context = TestDbContextFactory.Create();
        var existing = CreateActivity();
        context.Activities.Add(existing);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var missingId1 = Guid.NewGuid();
        var missingId2 = Guid.NewGuid();

        var result = await repository.GetMissingIdsAsync(
            [existing.Id, missingId1, missingId2],
            CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(missingId1, result);
        Assert.Contains(missingId2, result);
    }

    [Fact]
    public async Task GetMissingIdsAsync_ReturnsEmpty_WhenAllIdsExist()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);

        var result = await repository.GetMissingIdsAsync([activity.Id], CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ReturnsTrue_WhenThereArePendingChanges()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);
        await repository.AddAsync(CreateActivity(), CancellationToken.None);

        Assert.True(await repository.SaveChangesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task SaveChangesAsync_ReturnsFalse_WhenThereAreNoPendingChanges()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);

        Assert.False(await repository.SaveChangesAsync(CancellationToken.None));
    }
}
