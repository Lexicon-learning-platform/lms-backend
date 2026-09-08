using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// Covers only the members CourseRepository adds on top of RepositoryBase/RepositoryWithResourceBase,
// which already have their own test coverage.
public class CourseRepositoryTests
{
    private static readonly DateOnly BaseDate = new(2026, 1, 1);

    private static Course CreateCourse(
        string name = "Test course",
        string description = "Test description",
        DateOnly? startDate = null) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Description = description,
        StartDate = startDate ?? BaseDate,
        Duration = 30,
    };

    private static Module CreateModule() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Test module",
        Description = "Test description",
        Duration = 10,
    };

    private static Resource CreateResource() => new()
    {
        Id = Guid.NewGuid(),
        OwnerId = Guid.NewGuid(),
        Name = "Test resource",
        Description = "Test description",
        ResourceType = ResourceType.Text,
    };

    private static SearchParams NoFilters() => new(name: null, search: null);

    [Fact]
    public async Task GetCoursesAsync_OrdersByStartDateThenName_WhenNoFiltersApplied()
    {
        await using var context = TestDbContextFactory.Create();
        var first = CreateCourse(name: "B", startDate: BaseDate);
        var second = CreateCourse(name: "A", startDate: BaseDate);
        var third = CreateCourse(name: "C", startDate: BaseDate.AddDays(10));
        context.Courses.AddRange(first, second, third);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var (courses, _) = await repository.GetCoursesAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([second.Id, first.Id, third.Id], courses.Select(c => c.Id));
    }

    [Fact]
    public async Task GetCoursesAsync_FiltersByName()
    {
        await using var context = TestDbContextFactory.Create();
        var matching = CreateCourse(name: "Intro to Docker");
        var nonMatching = CreateCourse(name: "Intro to Git");
        context.Courses.AddRange(matching, nonMatching);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var searchParams = new SearchParams(name: "Docker", search: null);
        var (courses, _) = await repository.GetCoursesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([matching.Id], courses.Select(c => c.Id));
    }

    [Fact]
    public async Task GetCoursesAsync_FiltersBySearch_MatchingNameOrDescription()
    {
        await using var context = TestDbContextFactory.Create();
        var matchesByName = CreateCourse(name: "Docker basics", description: "unrelated");
        var matchesByDescription = CreateCourse(name: "unrelated", description: "Covers Docker networking");
        var nonMatching = CreateCourse(name: "Git basics", description: "unrelated");
        context.Courses.AddRange(matchesByName, matchesByDescription, nonMatching);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var searchParams = new SearchParams(name: null, search: "Docker");
        var (courses, _) = await repository.GetCoursesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(
            new[] { matchesByName.Id, matchesByDescription.Id }.OrderBy(id => id),
            courses.Select(c => c.Id).OrderBy(id => id));
    }

    [Fact]
    public async Task GetCoursesAsync_AppliesPagination_AndReturnsMatchingMetadata()
    {
        await using var context = TestDbContextFactory.Create();
        var courses = Enumerable.Range(0, 5)
            .Select(i => CreateCourse(name: $"Course {i}", startDate: BaseDate.AddDays(i)))
            .ToList();
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var (page, pagination) = await repository.GetCoursesAsync(NoFilters(), page: 2, pageSize: 2, CancellationToken.None);

        Assert.Equal([courses[2].Id, courses[3].Id], page.Select(c => c.Id));
        Assert.NotNull(pagination);
        Assert.Equal(5, pagination.TotalItemCount);
        Assert.Equal(3, pagination.TotalPageCount);
        Assert.Equal(2, pagination.PageSize);
        Assert.Equal(2, pagination.CurrentPage);
    }

    [Fact]
    public async Task GetCoursesReadOnlyAsync_ReturnsUntrackedEntities()
    {
        await using var context = TestDbContextFactory.Create();
        var course = CreateCourse();
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var (courses, _) = await repository.GetCoursesReadOnlyAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);
        var fetched = Assert.Single(courses);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Courses.AsNoTracking().Single(c => c.Id == course.Id);
        Assert.Equal(course.Name, stored.Name);
    }

    [Fact]
    public async Task GetCourseAsync_ReturnsCourseWithModulesAndResources_WhenExists()
    {
        await using var context = TestDbContextFactory.Create();
        var course = CreateCourse();
        var module = CreateModule();
        var resource = CreateResource();
        context.Courses.Add(course);
        context.Modules.Add(module);
        context.Resources.Add(resource);
        context.CourseModules.Add(new CourseModule { CourseId = course.Id, ModuleId = module.Id });
        context.CourseResources.Add(new CourseResource { CourseId = course.Id, ResourceId = resource.Id });
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var result = await repository.GetCourseAsync(course.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        var attachedModule = Assert.Single(result.Modules);
        Assert.Equal(module.Id, attachedModule.ModuleId);
        var attachedResource = Assert.Single(result.Resources);
        Assert.Equal(resource.Id, attachedResource.ResourceId);
    }

    [Fact]
    public async Task GetCourseAsync_ReturnsNull_WhenNotExists()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new CourseRepository(context);

        var result = await repository.GetCourseAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCourseReadOnlyAsync_ReturnsUntrackedEntity()
    {
        await using var context = TestDbContextFactory.Create();
        var course = CreateCourse();
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        var repository = new CourseRepository(context);
        var fetched = await repository.GetCourseReadOnlyAsync(course.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Courses.AsNoTracking().Single(c => c.Id == course.Id);
        Assert.Equal(course.Name, stored.Name);
    }
}
