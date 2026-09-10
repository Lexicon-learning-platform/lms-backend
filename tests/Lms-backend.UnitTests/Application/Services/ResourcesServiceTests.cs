using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;
using Lms_backend.Application.Services;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.UnitTests.Application.Services;

public class ResourcesServiceTests
{
    private static readonly Guid OwnerId = Guid.NewGuid();

    private static ApplicationUser Owner(Guid? id = null) => new()
    {
        Id = id ?? OwnerId,
        UserName = "test.user",
        GivenName = "Test",
        LastName = "User",
    };

    private static Resource NewResource(Guid? id = null, Guid? ownerId = null, ApplicationUser? owner = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        OwnerId = ownerId ?? OwnerId,
        Owner = owner ?? Owner(ownerId ?? OwnerId),
        Name = "Existing resource",
        Description = "Existing description",
        ResourceType = ResourceType.Text,
        Data = "Existing data",
    };

    private static ResourceForChangeDto ValidChangeDto() => new()
    {
        Name = "Docker cheat sheet",
        Description = "Covers Docker basics",
        Type = ResourceType.URL,
        Data = "https://example.com",
    };

    // --- Create ---

    [Fact]
    public async Task Create_AddsEntityAndReturnsRefetchedDto_WhenDataIsValid()
    {
        var repository = new FakeResourceRepository();
        var owner = Owner();
        repository.GetResourceReadOnlyHandler = id => new Resource
        {
            Id = id,
            OwnerId = owner.Id,
            Owner = owner,
            Name = "Docker cheat sheet",
            Description = "Covers Docker basics",
            ResourceType = ResourceType.URL,
            Data = "https://example.com",
        };
        var service = new ResourcesService(repository);
        var data = ValidChangeDto();

        var result = await service.Create(data, owner.Id, canModerate: false);

        var added = Assert.Single(repository.AddedEntities);
        Assert.Equal(owner.Id, added.OwnerId);
        Assert.Equal(data.Name, added.Name);
        Assert.Equal(data.Description, added.Description);
        Assert.Equal(data.Type, added.ResourceType);
        Assert.Equal(data.Data, added.Data);
        Assert.Equal(1, repository.SaveChangesCallCount);

        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Description, result.Description);
        Assert.Equal(data.Type, result.Type);
        Assert.Equal(owner.Id, result.CreatedBy.Id);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_AndDoesNotTouchRepository_WhenNameIsTooShort()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(data, OwnerId, canModerate: false));

        Assert.Empty(repository.AddedEntities);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("this name is definitely far too long to be considered valid by the validator")]
    public async Task Create_ThrowsValidationException_WhenNameLengthIsOutOfRange(string invalidName)
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);
        var data = ValidChangeDto();
        data.Name = invalidName;

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(data, OwnerId, canModerate: false));
    }

    // --- GetMany ---

    [Fact]
    public async Task GetMany_UsesReadOnlyRepositoryAndReturnsMappedDtosWithPagination()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        var pagination = new PaginationMetadata(totalItemCount: 1, pageSize: 10, currentPage: 1);
        repository.GetResourcesReadOnlyResult = ([resource], pagination);
        var service = new ResourcesService(repository);
        var searchParams = new ResourceSearchParams(name: null, search: null, type: null);

        var (dtos, returnedPagination) = await service.GetMany(searchParams, page: 1, pageSize: 10);

        var dto = Assert.Single(dtos);
        Assert.Equal(resource.Id, dto.Id);
        Assert.Same(pagination, returnedPagination);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetMany_DefaultsPageToOne_WhenPageIsNullOrLessThanOne(int? page)
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await service.GetMany(new ResourceSearchParams(null, null, null), page: page, pageSize: 20);

        Assert.Equal(1, repository.LastGetResourcesReadOnlyCall!.Value.Page);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPage_WhenValid()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await service.GetMany(new ResourceSearchParams(null, null, null), page: 3, pageSize: 20);

        Assert.Equal(3, repository.LastGetResourcesReadOnlyCall!.Value.Page);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    [InlineData(10)]
    public async Task GetMany_DefaultsPageSizeToTen_WhenPageSizeIsNullOrAtMostTen(int? pageSize)
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await service.GetMany(new ResourceSearchParams(null, null, null), page: 1, pageSize: pageSize);

        Assert.Equal(10, repository.LastGetResourcesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPageSize_WhenGreaterThanTen()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await service.GetMany(new ResourceSearchParams(null, null, null), page: 1, pageSize: 25);

        Assert.Equal(25, repository.LastGetResourcesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_PassesSearchParamsThrough()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);
        var searchParams = new ResourceSearchParams(name: "Docker", search: null, type: ResourceType.URL);

        await service.GetMany(searchParams, page: 1, pageSize: 20);

        Assert.Same(searchParams, repository.LastGetResourcesReadOnlyCall!.Value.SearchParams);
    }

    // --- GetOne ---

    [Fact]
    public async Task GetOne_ReturnsMappedDto_WhenResourceExists()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.ReadOnlyResources.Add(resource);
        var service = new ResourcesService(repository);

        var result = await service.GetOne(resource.Id);

        Assert.Equal(resource.Id, result.Id);
        Assert.Equal(resource.Name, result.Name);
        Assert.Equal(resource.OwnerId, result.CreatedBy.Id);
    }

    [Fact]
    public async Task GetOne_ThrowsNotFoundException_WhenResourceDoesNotExist()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);
        var id = Guid.NewGuid();

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetOne(id));
        Assert.Contains(id.ToString(), ex.Message);
    }

    // --- Remove ---

    [Fact]
    public async Task Remove_DeletesAndSaves_WhenUserIsOwner()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);

        await service.Remove(resource.Id, resource.OwnerId, canModerate: false);

        Assert.Equal([resource], repository.DeletedEntities);
        Assert.Equal(1, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_DeletesAndSaves_WhenUserIsNotOwnerButCanModerate()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);

        await service.Remove(resource.Id, Guid.NewGuid(), canModerate: true);

        Assert.Equal([resource], repository.DeletedEntities);
        Assert.Equal(1, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_ThrowsForbiddenException_AndDoesNotDelete_WhenUserIsNotOwnerAndCannotModerate()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);

        await Assert.ThrowsAsync<ForbiddenException>(() => service.Remove(resource.Id, Guid.NewGuid(), canModerate: false));

        Assert.Empty(repository.DeletedEntities);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_DoesNothing_WhenResourceDoesNotExist()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await service.Remove(Guid.NewGuid(), Guid.NewGuid(), canModerate: false);

        Assert.Empty(repository.DeletedEntities);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    // --- Update (ResourceForChangeDto) ---

    [Fact]
    public async Task Update_AppliesChangesAndSaves_WhenUserIsOwner()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var data = ValidChangeDto();

        await service.Update(resource.Id, data, resource.OwnerId, canModerate: false);

        Assert.Equal(data.Name, resource.Name);
        Assert.Equal(data.Description, resource.Description);
        Assert.Equal(data.Type, resource.ResourceType);
        Assert.Equal(data.Data, resource.Data);
        Assert.Equal(1, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_AppliesChangesAndSaves_WhenUserIsNotOwnerButCanModerate()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var data = ValidChangeDto();

        await service.Update(resource.Id, data, Guid.NewGuid(), canModerate: true);

        Assert.Equal(data.Name, resource.Name);
        Assert.Equal(1, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsForbiddenException_AndDoesNotSave_WhenUserIsNotOwnerAndCannotModerate()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var originalName = resource.Name;

        await Assert.ThrowsAsync<ForbiddenException>(() => service.Update(resource.Id, ValidChangeDto(), Guid.NewGuid(), canModerate: false));

        Assert.Equal(originalName, resource.Name);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsNotFoundException_WhenResourceDoesNotExist()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), ValidChangeDto(), Guid.NewGuid(), canModerate: false));
    }

    [Fact]
    public async Task Update_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var originalName = resource.Name;
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(resource.Id, data, resource.OwnerId, canModerate: false));

        Assert.Equal(originalName, resource.Name);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    // --- Update (JsonPatchDocument) ---

    [Fact]
    public async Task Update_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves_WhenUserIsOwner()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await service.Update(resource.Id, patch, resource.OwnerId, canModerate: false);

        Assert.Equal("Patched name", resource.Name);
        Assert.Equal("Existing description", resource.Description);
        Assert.Equal(1, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsForbiddenException_AndDoesNotSave_WhenUserIsNotOwnerAndCannotModerate()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await Assert.ThrowsAsync<ForbiddenException>(() => service.Update(resource.Id, patch, Guid.NewGuid(), canModerate: false));

        Assert.Equal("Existing resource", resource.Name);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsNotFoundException_WhenResourceDoesNotExist()
    {
        var repository = new FakeResourceRepository();
        var service = new ResourcesService(repository);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), patch, Guid.NewGuid(), canModerate: false));
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsValidationException_WhenPatchProducesInvalidData()
    {
        var repository = new FakeResourceRepository();
        var resource = NewResource();
        repository.TrackedResources.Add(resource);
        var service = new ResourcesService(repository);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "ab");

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(resource.Id, patch, resource.OwnerId, canModerate: false));

        Assert.Equal("Existing resource", resource.Name);
        Assert.Equal(0, repository.SaveChangesCallCount);
    }
}
