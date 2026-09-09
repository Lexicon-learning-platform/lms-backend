using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// Covers only the members ModuleRepository adds on top of RepositoryBase/RepositoryWithResourceBase,
// which already have their own test coverage.
public class ModuleRepositoryTests
{
    private static Module CreateModule(
        string name = "Test module",
        string description = "Test description") => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Description = description,
        Duration = 10,
    };

    private static Activity CreateActivity(Guid moduleId) => new()
    {
        Id = Guid.NewGuid(),
        ModuleId = moduleId,
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

    private static ModuleSearchParams NoFilters() => new(name: null, search: null, courseId: null);

    [Fact]
    public async Task GetModulesAsync_OrdersByName_WhenNoFiltersApplied()
    {
        await using var context = TestDbContextFactory.Create();
        var first = CreateModule(name: "B");
        var second = CreateModule(name: "A");
        context.Modules.AddRange(first, second);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var (modules, _) = await repository.GetModulesAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([second.Id, first.Id], modules.Select(m => m.Id));
    }

    [Fact]
    public async Task GetModulesAsync_FiltersByName()
    {
        await using var context = TestDbContextFactory.Create();
        var matching = CreateModule(name: "Intro to Docker");
        var nonMatching = CreateModule(name: "Intro to Git");
        context.Modules.AddRange(matching, nonMatching);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var searchParams = new ModuleSearchParams(name: "Docker", search: null, courseId: null);
        var (modules, _) = await repository.GetModulesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([matching.Id], modules.Select(m => m.Id));
    }

    [Fact]
    public async Task GetModulesAsync_FiltersBySearch_MatchingNameOrDescription()
    {
        await using var context = TestDbContextFactory.Create();
        var matchesByName = CreateModule(name: "Docker basics", description: "unrelated");
        var matchesByDescription = CreateModule(name: "unrelated", description: "Covers Docker networking");
        var nonMatching = CreateModule(name: "Git basics", description: "unrelated");
        context.Modules.AddRange(matchesByName, matchesByDescription, nonMatching);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var searchParams = new ModuleSearchParams(name: null, search: "Docker", courseId: null);
        var (modules, _) = await repository.GetModulesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(
            new[] { matchesByName.Id, matchesByDescription.Id }.OrderBy(id => id),
            modules.Select(m => m.Id).OrderBy(id => id));
    }

    [Fact]
    public async Task GetModulesAsync_FiltersByCourseId_AndOrdersByStartTimeOffsetThenName()
    {
        await using var context = TestDbContextFactory.Create();
        var courseId = Guid.NewGuid();
        var otherCourseId = Guid.NewGuid();
        var later = CreateModule(name: "Later module");
        var earlier = CreateModule(name: "Earlier module");
        var otherCourseModule = CreateModule(name: "Other course module");
        context.Modules.AddRange(later, earlier, otherCourseModule);
        context.CourseModules.AddRange(
            new CourseModule { CourseId = courseId, ModuleId = later.Id, StartTimeOffset = 10 },
            new CourseModule { CourseId = courseId, ModuleId = earlier.Id, StartTimeOffset = 0 },
            new CourseModule { CourseId = otherCourseId, ModuleId = otherCourseModule.Id, StartTimeOffset = 0 });
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var searchParams = new ModuleSearchParams(name: null, search: null, courseId: courseId);
        var (modules, _) = await repository.GetModulesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([earlier.Id, later.Id], modules.Select(m => m.Id));
    }

    [Fact]
    public async Task GetModulesAsync_AppliesPagination_AndReturnsMatchingMetadata()
    {
        await using var context = TestDbContextFactory.Create();
        var modules = Enumerable.Range(0, 5)
            .Select(i => CreateModule(name: $"Module {i}"))
            .OrderBy(m => m.Name)
            .ToList();
        context.Modules.AddRange(modules);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var (page, pagination) = await repository.GetModulesAsync(NoFilters(), page: 2, pageSize: 2, CancellationToken.None);

        Assert.Equal([modules[2].Id, modules[3].Id], page.Select(m => m.Id));
        Assert.NotNull(pagination);
        Assert.Equal(5, pagination.TotalItemCount);
        Assert.Equal(3, pagination.TotalPageCount);
        Assert.Equal(2, pagination.PageSize);
        Assert.Equal(2, pagination.CurrentPage);
    }

    [Fact]
    public async Task GetModulesReadOnlyAsync_ReturnsUntrackedEntities()
    {
        await using var context = TestDbContextFactory.Create();
        var module = CreateModule();
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var (modules, _) = await repository.GetModulesReadOnlyAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);
        var fetched = Assert.Single(modules);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Modules.AsNoTracking().Single(m => m.Id == module.Id);
        Assert.Equal(module.Name, stored.Name);
    }

    [Fact]
    public async Task GetModuleAsync_ReturnsModuleWithActivitiesAndResources_WhenExists()
    {
        await using var context = TestDbContextFactory.Create();
        var module = CreateModule();
        var activity = CreateActivity(module.Id);
        var resource = CreateResource();
        context.Modules.Add(module);
        context.Activities.Add(activity);
        context.Resources.Add(resource);
        context.ModuleResources.Add(new ModuleResource { ModuleId = module.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var result = await repository.GetModuleAsync(module.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(module.Id, result.Id);
        var loadedActivity = Assert.Single(result.Activities);
        Assert.Equal(activity.Id, loadedActivity.Id);
        var attachedResource = Assert.Single(result.Resources);
        Assert.Equal(resource.Id, attachedResource.ResourceId);
    }

    [Fact]
    public async Task GetModuleAsync_ReturnsNull_WhenNotExists()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ModuleRepository(context);

        var result = await repository.GetModuleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetModuleReadOnlyAsync_ReturnsUntrackedEntity()
    {
        await using var context = TestDbContextFactory.Create();
        var module = CreateModule();
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var repository = new ModuleRepository(context);
        var fetched = await repository.GetModuleReadOnlyAsync(module.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Modules.AsNoTracking().Single(m => m.Id == module.Id);
        Assert.Equal(module.Name, stored.Name);
    }
}
