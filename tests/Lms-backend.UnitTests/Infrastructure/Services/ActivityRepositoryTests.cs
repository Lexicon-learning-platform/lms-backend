using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// Covers only the members ActivityRepository adds on top of RepositoryBase/RepositoryWithResourceBase,
// which already have their own test coverage.
public class ActivityRepositoryTests
{
    private static Activity CreateActivity(
        string name = "Test activity",
        string description = "Test description",
        ActivityType type = ActivityType.Lecture,
        int startTimeOffset = 0) => new()
    {
        Id = Guid.NewGuid(),
        ModuleId = Guid.NewGuid(),
        ActivityType = type,
        Name = name,
        Description = description,
        StartTimeOffset = startTimeOffset,
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

    private static ActivitySearchParams NoFilters() => new(name: null, search: null, type: null);

    [Fact]
    public async Task GetActivitiesAsync_OrdersByStartTimeOffsetThenName_WhenNoFiltersApplied()
    {
        await using var context = TestDbContextFactory.Create();
        var first = CreateActivity(name: "B", startTimeOffset: 0);
        var second = CreateActivity(name: "A", startTimeOffset: 0);
        var third = CreateActivity(name: "C", startTimeOffset: 10);
        context.Activities.AddRange(first, second, third);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var (activities, _) = await repository.GetActivitiesAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([second.Id, first.Id, third.Id], activities.Select(a => a.Id));
    }

    [Fact]
    public async Task GetActivitiesAsync_FiltersByName()
    {
        await using var context = TestDbContextFactory.Create();
        var matching = CreateActivity(name: "Intro to Docker");
        var nonMatching = CreateActivity(name: "Intro to Git");
        context.Activities.AddRange(matching, nonMatching);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var searchParams = new ActivitySearchParams(name: "Docker", search: null, type: null);
        var (activities, _) = await repository.GetActivitiesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([matching.Id], activities.Select(a => a.Id));
    }

    [Fact]
    public async Task GetActivitiesAsync_FiltersBySearch_MatchingNameOrDescription()
    {
        await using var context = TestDbContextFactory.Create();
        var matchesByName = CreateActivity(name: "Docker basics", description: "unrelated");
        var matchesByDescription = CreateActivity(name: "unrelated", description: "Covers Docker networking");
        var nonMatching = CreateActivity(name: "Git basics", description: "unrelated");
        context.Activities.AddRange(matchesByName, matchesByDescription, nonMatching);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var searchParams = new ActivitySearchParams(name: null, search: "Docker", type: null);
        var (activities, _) = await repository.GetActivitiesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(
            new[] { matchesByName.Id, matchesByDescription.Id }.OrderBy(id => id),
            activities.Select(a => a.Id).OrderBy(id => id));
    }

    [Fact]
    public async Task GetActivitiesAsync_FiltersByType()
    {
        await using var context = TestDbContextFactory.Create();
        var lecture = CreateActivity(type: ActivityType.Lecture);
        var exercise = CreateActivity(type: ActivityType.Exercise);
        context.Activities.AddRange(lecture, exercise);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var searchParams = new ActivitySearchParams(name: null, search: null, type: ActivityType.Exercise);
        var (activities, _) = await repository.GetActivitiesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([exercise.Id], activities.Select(a => a.Id));
    }

    [Fact]
    public async Task GetActivitiesAsync_AppliesPagination_AndReturnsMatchingMetadata()
    {
        await using var context = TestDbContextFactory.Create();
        var activities = Enumerable.Range(0, 5)
            .Select(i => CreateActivity(name: $"Activity {i}", startTimeOffset: i))
            .ToList();
        context.Activities.AddRange(activities);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var (page, pagination) = await repository.GetActivitiesAsync(NoFilters(), page: 2, pageSize: 2, CancellationToken.None);

        Assert.Equal([activities[2].Id, activities[3].Id], page.Select(a => a.Id));
        Assert.NotNull(pagination);
        Assert.Equal(5, pagination.TotalItemCount);
        Assert.Equal(3, pagination.TotalPageCount);
        Assert.Equal(2, pagination.PageSize);
        Assert.Equal(2, pagination.CurrentPage);
    }

    [Fact]
    public async Task GetActivitiesReadOnlyAsync_ReturnsUntrackedEntities()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var (activities, _) = await repository.GetActivitiesReadOnlyAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);
        var fetched = Assert.Single(activities);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Activities.AsNoTracking().Single(a => a.Id == activity.Id);
        Assert.Equal(activity.Name, stored.Name);
    }

    [Fact]
    public async Task GetActivityAsync_ReturnsActivityWithResources_WhenExists()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        var resource = CreateResource();
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        context.ActivityResources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var result = await repository.GetActivityAsync(activity.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(activity.Id, result.Id);
        var attachedResource = Assert.Single(result.Resources);
        Assert.Equal(resource.Id, attachedResource.ResourceId);
    }

    [Fact]
    public async Task GetActivityAsync_ReturnsNull_WhenNotExists()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ActivityRepository(context);

        var result = await repository.GetActivityAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetActivityReadOnlyAsync_ReturnsUntrackedEntity()
    {
        await using var context = TestDbContextFactory.Create();
        var activity = CreateActivity();
        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var repository = new ActivityRepository(context);
        var fetched = await repository.GetActivityReadOnlyAsync(activity.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Activities.AsNoTracking().Single(a => a.Id == activity.Id);
        Assert.Equal(activity.Name, stored.Name);
    }
}
