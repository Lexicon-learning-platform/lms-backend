using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure;
using Lms_backend.Infrastructure.Models;
using Lms_backend.Infrastructure.Services;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.UnitTests.Infrastructure.Services;

// Covers only the members ResourceRepository adds on top of RepositoryBase, which already has
// its own test coverage. ResourceRepository does not extend RepositoryWithResourceBase.
//
// ResourceRepository always Include()s Owner, and EF treats that relationship as required
// (non-nullable OwnerId), so every Resource here needs a real, matching ApplicationUser row -
// otherwise the required-navigation join filters the resource out of the query entirely.
public class ResourceRepositoryTests
{
    private static readonly DateTime BaseTime = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static Resource AddResource(
        AppDbContext context,
        string name = "Test resource",
        string description = "Test description",
        ResourceType type = ResourceType.Text,
        DateTime? createdAt = null)
    {
        var owner = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = $"test.user.{Guid.NewGuid()}",
            GivenName = "Test",
            LastName = "User",
        };
        var resource = new Resource
        {
            Id = Guid.NewGuid(),
            OwnerId = owner.Id,
            Name = name,
            Description = description,
            ResourceType = type,
            CreatedAt = createdAt ?? BaseTime,
        };

        context.Users.Add(owner);
        context.Resources.Add(resource);
        return resource;
    }

    private static ResourceSearchParams NoFilters() => new(name: null, search: null, type: null);

    [Fact]
    public async Task GetResourcesAsync_OrdersByCreatedAtThenName_WhenNoFiltersApplied()
    {
        await using var context = TestDbContextFactory.Create();
        var first = AddResource(context, name: "B", createdAt: BaseTime);
        var second = AddResource(context, name: "A", createdAt: BaseTime);
        var third = AddResource(context, name: "C", createdAt: BaseTime.AddDays(1));
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var (resources, _) = await repository.GetResourcesAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([second.Id, first.Id, third.Id], resources.Select(r => r.Id));
    }

    [Fact]
    public async Task GetResourcesAsync_FiltersByName()
    {
        await using var context = TestDbContextFactory.Create();
        var matching = AddResource(context, name: "Docker cheat sheet");
        AddResource(context, name: "Git cheat sheet");
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var searchParams = new ResourceSearchParams(name: "Docker", search: null, type: null);
        var (resources, _) = await repository.GetResourcesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([matching.Id], resources.Select(r => r.Id));
    }

    [Fact]
    public async Task GetResourcesAsync_FiltersBySearch_MatchingNameOrDescription()
    {
        await using var context = TestDbContextFactory.Create();
        var matchesByName = AddResource(context, name: "Docker basics", description: "unrelated");
        var matchesByDescription = AddResource(context, name: "unrelated", description: "Covers Docker networking");
        AddResource(context, name: "Git basics", description: "unrelated");
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var searchParams = new ResourceSearchParams(name: null, search: "Docker", type: null);
        var (resources, _) = await repository.GetResourcesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(
            new[] { matchesByName.Id, matchesByDescription.Id }.OrderBy(id => id),
            resources.Select(r => r.Id).OrderBy(id => id));
    }

    [Fact]
    public async Task GetResourcesAsync_FiltersByType()
    {
        await using var context = TestDbContextFactory.Create();
        AddResource(context, type: ResourceType.Text);
        var url = AddResource(context, type: ResourceType.URL);
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var searchParams = new ResourceSearchParams(name: null, search: null, type: ResourceType.URL);
        var (resources, _) = await repository.GetResourcesAsync(searchParams, page: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal([url.Id], resources.Select(r => r.Id));
    }

    [Fact]
    public async Task GetResourcesAsync_AppliesPagination_AndReturnsMatchingMetadata()
    {
        await using var context = TestDbContextFactory.Create();
        var resources = Enumerable.Range(0, 5)
            .Select(i => AddResource(context, name: $"Resource {i}", createdAt: BaseTime.AddMinutes(i)))
            .ToList();
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var (page, pagination) = await repository.GetResourcesAsync(NoFilters(), page: 2, pageSize: 2, CancellationToken.None);

        Assert.Equal([resources[2].Id, resources[3].Id], page.Select(r => r.Id));
        Assert.NotNull(pagination);
        Assert.Equal(5, pagination.TotalItemCount);
        Assert.Equal(3, pagination.TotalPageCount);
        Assert.Equal(2, pagination.PageSize);
        Assert.Equal(2, pagination.CurrentPage);
    }

    [Fact]
    public async Task GetResourcesReadOnlyAsync_ReturnsUntrackedEntities()
    {
        await using var context = TestDbContextFactory.Create();
        var resource = AddResource(context);
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var (resources, _) = await repository.GetResourcesReadOnlyAsync(NoFilters(), page: 1, pageSize: 10, CancellationToken.None);
        var fetched = Assert.Single(resources);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Resources.AsNoTracking().Single(r => r.Id == resource.Id);
        Assert.Equal(resource.Name, stored.Name);
    }

    [Fact]
    public async Task GetResourceAsync_ReturnsResourceWithOwner_WhenExists()
    {
        await using var context = TestDbContextFactory.Create();
        var resource = AddResource(context);
        var ownerId = resource.OwnerId;
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var result = await repository.GetResourceAsync(resource.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(resource.Id, result.Id);
        Assert.NotNull(result.Owner);
        Assert.Equal(ownerId, result.Owner.Id);
    }

    [Fact]
    public async Task GetResourceAsync_ReturnsNull_WhenNotExists()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ResourceRepository(context);

        var result = await repository.GetResourceAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetResourceReadOnlyAsync_ReturnsUntrackedEntity()
    {
        await using var context = TestDbContextFactory.Create();
        var resource = AddResource(context);
        await context.SaveChangesAsync();

        var repository = new ResourceRepository(context);
        var fetched = await repository.GetResourceReadOnlyAsync(resource.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        fetched.Name = "Changed";
        await context.SaveChangesAsync();

        var stored = context.Resources.AsNoTracking().Single(r => r.Id == resource.Id);
        Assert.Equal(resource.Name, stored.Name);
    }
}
