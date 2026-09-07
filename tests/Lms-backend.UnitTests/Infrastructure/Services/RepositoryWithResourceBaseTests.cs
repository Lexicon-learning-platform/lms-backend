using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// RepositoryWithResourceBase<TEntity, TJoin> is abstract, so it is exercised here through
// ActivityRepository, which adds no overrides of the members under test.
public class RepositoryWithResourceBaseTests
{
    private static Activity CreateActivity() => new()
    {
        Id = Guid.NewGuid(),
        ModuleId = Guid.NewGuid(),
        ActivityType = ActivityType.Lecture,
        Name = "Test activity",
        Description = "Test description",
        StartTimeOffset = 0,
        DurationMinutes = 30,
    };

    private static Resource CreateResource() => new()
    {
        Id = Guid.NewGuid(),
        OwnerId = Guid.NewGuid(),
        Name = "Test resource",
        Description = "Test description",
        ResourceType = ResourceType.Text,
    };

    [Fact]
    public async Task GetResourcesAsync_ReturnsAttachedResources()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        context.ActivityResources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var result = await repository.GetResourcesAsync(activity.Id, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(resource.Id, result[0].Id);
    }

    [Fact]
    public async Task GetResourcesAsync_ReturnsEmpty_WhenNoResourcesAttached()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var result = await repository.GetResourcesAsync(activity.Id, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AttachResourceAsync_AddsJoin_AndReturnsTrue_WhenNotAlreadyAttached()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var attached = await repository.AttachResourceAsync(activity.Id, resource.Id, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.True(attached);
        Assert.Contains(context.ActivityResources, j => j.ActivityId == activity.Id && j.ResourceId == resource.Id);
    }

    [Fact]
    public async Task AttachResourceAsync_ReturnsFalse_AndDoesNotDuplicate_WhenAlreadyAttached()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        context.ActivityResources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var attached = await repository.AttachResourceAsync(activity.Id, resource.Id, CancellationToken.None);

        Assert.False(attached);
        Assert.Single(context.ActivityResources.Where(j => j.ActivityId == activity.Id && j.ResourceId == resource.Id));
    }

    [Fact]
    public async Task DetachResourceAsync_RemovesJoin_WhenAttached()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        context.ActivityResources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        await repository.DetachResourceAsync(activity.Id, resource.Id, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.DoesNotContain(context.ActivityResources, j => j.ActivityId == activity.Id && j.ResourceId == resource.Id);
    }

    [Fact]
    public async Task DetachResourceAsync_DoesNothing_WhenNotAttached()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        await repository.DetachResourceAsync(activity.Id, resource.Id, CancellationToken.None);

        Assert.False(await repository.SaveChangesAsync(CancellationToken.None));
    }
}
