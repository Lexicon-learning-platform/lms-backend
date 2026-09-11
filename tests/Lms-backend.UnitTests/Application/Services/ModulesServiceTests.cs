using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;
using Lms_backend.Application.Services;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.UnitTests.Application.Services;

public class ModulesServiceTests
{
    private static Module NewModule(
        Guid? id = null,
        string name = "Existing module",
        string description = "Existing description",
        int duration = 30) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name,
        Description = description,
        Duration = duration,
    };

    private static Resource NewResource(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        OwnerId = Guid.NewGuid(),
        Owner = new ApplicationUser { Id = Guid.NewGuid(), UserName = "test.user", GivenName = "Test", LastName = "User" },
        Name = "Existing resource",
        Description = "Existing description",
        ResourceType = ResourceType.Text,
        Data = "Existing data",
    };

    private static ModuleForChangeDto ValidChangeDto() => new()
    {
        Name = "Docker fundamentals",
        Description = "Covers Docker basics",
        Duration = 90,
    };

    private static ResourceForChangeDto ValidResourceChangeDto() => new()
    {
        Name = "Docker cheat sheet",
        Description = "Covers Docker basics",
        Type = ResourceType.URL,
        Data = "https://example.com",
    };

    private static (FakeModuleRepository, FakeResourceRepository, ModulesService) CreateService()
    {
        var moduleRepository = new FakeModuleRepository();
        var resourceRepository = new FakeResourceRepository();
        var service = new ModulesService(moduleRepository, resourceRepository);
        return (moduleRepository, resourceRepository, service);
    }

    // --- GetResources ---

    [Fact]
    public async Task GetResources_ReturnsMappedDtos_WhenModuleExists()
    {
        var (moduleRepository, _, service) = CreateService();
        var resource = NewResource();
        var module = NewModule();
        module.Resources.Add(new ModuleResource { ModuleId = module.Id, ResourceId = resource.Id, Resource = resource });
        moduleRepository.ReadOnlyModules.Add(module);

        var result = await service.GetResources(module.Id);

        var dto = Assert.Single(result);
        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResources_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResources(Guid.NewGuid()));
    }

    // --- GetResource ---

    [Fact]
    public async Task GetResource_ReturnsMappedDto_WhenResourceIsAttached()
    {
        var (moduleRepository, _, service) = CreateService();
        var resource = NewResource();
        var module = NewModule();
        module.Resources.Add(new ModuleResource { ModuleId = module.Id, ResourceId = resource.Id, Resource = resource });
        moduleRepository.ReadOnlyModules.Add(module);

        var dto = await service.GetResource(module.Id, resource.Id);

        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenResourceIsNotAttached()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.ReadOnlyModules.Add(module);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(module.Id, Guid.NewGuid()));
    }

    // --- AddResource ---

    [Fact]
    public async Task AddResource_AddsAttachesSavesOnModuleRepository_AndReturnsRefetchedDto()
    {
        var (moduleRepository, resourceRepository, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var userId = Guid.NewGuid();
        resourceRepository.GetResourceReadOnlyHandler = id => new Resource
        {
            Id = id,
            OwnerId = userId,
            Owner = new ApplicationUser { Id = userId, UserName = "test.user", GivenName = "Test", LastName = "User" },
            Name = "Docker cheat sheet",
            Description = "Covers Docker basics",
            ResourceType = ResourceType.URL,
            Data = "https://example.com",
        };
        var data = ValidResourceChangeDto();

        var result = await service.AddResource(module.Id, userId, data);

        var added = Assert.Single(resourceRepository.AddedEntities);
        Assert.Equal(userId, added.OwnerId);
        Assert.Equal(data.Name, added.Name);

        var (ModuleId, ResourceId) = Assert.Single(moduleRepository.AttachResourceCalls);
        Assert.Equal(module.Id, ModuleId);
        Assert.Equal(added.Id, ResourceId);

        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(userId, result.CreatedBy.Id);
    }

    [Fact]
    public async Task AddResource_ThrowsValidationException_AndDoesNotAddOrAttach_WhenDataIsInvalid()
    {
        var (moduleRepository, resourceRepository, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.AddResource(module.Id, Guid.NewGuid(), data));

        Assert.Empty(resourceRepository.AddedEntities);
        Assert.Empty(moduleRepository.AttachResourceCalls);
    }

    [Fact]
    public async Task AddResource_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AddResource(Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    // --- AttachResource ---

    [Fact]
    public async Task AttachResource_AttachesAndSaves_WhenNotAlreadyAttached()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        moduleRepository.AttachResourceResult = true;
        var resourceId = Guid.NewGuid();

        var attached = await service.AttachResource(module.Id, resourceId);

        Assert.True(attached);
        Assert.Equal([(module.Id, resourceId)], moduleRepository.AttachResourceCalls);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_DoesNotSave_WhenAlreadyAttached()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        moduleRepository.AttachResourceResult = false;

        var attached = await service.AttachResource(module.Id, Guid.NewGuid());

        Assert.False(attached);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AttachResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Create ---

    [Fact]
    public async Task Create_AddsEntityAndReturnsMappedDto_WhenDataIsValid()
    {
        var (moduleRepository, _, service) = CreateService();
        var data = ValidChangeDto();

        var result = await service.Create(data);

        var added = Assert.Single(moduleRepository.AddedEntities);
        Assert.Equal(data.Name, added.Name);
        Assert.Equal(data.Description, added.Description);
        Assert.Equal(data.Duration, added.Duration);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_AndDoesNotAdd_WhenDataIsInvalid()
    {
        var (moduleRepository, _, service) = CreateService();
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(data));

        Assert.Empty(moduleRepository.AddedEntities);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_WhenDurationIsNegative()
    {
        var (moduleRepository, _, service) = CreateService();
        var data = ValidChangeDto();
        data.Duration = -1;

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(data));

        Assert.Empty(moduleRepository.AddedEntities);
    }

    // --- GetMany ---

    [Fact]
    public async Task GetMany_UsesReadOnlyRepositoryAndReturnsMappedDtosWithPagination()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        var pagination = new PaginationMetadata(totalItemCount: 1, pageSize: 10, currentPage: 1);
        moduleRepository.GetModulesReadOnlyResult = ([module], pagination);
        var searchParams = new ModuleSearchParams(name: null, search: null, courseId: null);

        var (dtos, returnedPagination) = await service.GetMany(searchParams, page: 1, pageSize: 10);

        var dto = Assert.Single(dtos);
        Assert.Equal(module.Id, dto.Id);
        Assert.Same(pagination, returnedPagination);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetMany_DefaultsPageToOne_WhenPageIsNullOrLessThanOne(int? page)
    {
        var (moduleRepository, _, service) = CreateService();

        await service.GetMany(new ModuleSearchParams(null, null, null), page: page, pageSize: 20);

        Assert.Equal(1, moduleRepository.LastGetModulesReadOnlyCall!.Value.Page);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPage_WhenValid()
    {
        var (moduleRepository, _, service) = CreateService();

        await service.GetMany(new ModuleSearchParams(null, null, null), page: 3, pageSize: 20);

        Assert.Equal(3, moduleRepository.LastGetModulesReadOnlyCall!.Value.Page);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task GetMany_DefaultsPageSizeToTen_WhenPageSizeIsNullOrNotPositive(int? pageSize)
    {
        var (moduleRepository, _, service) = CreateService();

        await service.GetMany(new ModuleSearchParams(null, null, null), page: 1, pageSize: pageSize);

        Assert.Equal(10, moduleRepository.LastGetModulesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPageSize_WhenPositive()
    {
        var (moduleRepository, _, service) = CreateService();

        await service.GetMany(new ModuleSearchParams(null, null, null), page: 1, pageSize: 1);

        Assert.Equal(1, moduleRepository.LastGetModulesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_PassesSearchParamsThrough()
    {
        var (moduleRepository, _, service) = CreateService();
        var searchParams = new ModuleSearchParams(name: "Docker", search: null, courseId: Guid.NewGuid());

        await service.GetMany(searchParams, page: 1, pageSize: 20);

        Assert.Same(searchParams, moduleRepository.LastGetModulesReadOnlyCall!.Value.SearchParams);
    }

    // --- GetOne ---

    [Fact]
    public async Task GetOne_ReturnsExtendedDto_WhenModuleExists()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.ReadOnlyModules.Add(module);

        var result = await service.GetOne(module.Id);

        Assert.Equal(module.Id, result.Id);
        Assert.Equal(module.Name, result.Name);
    }

    [Fact]
    public async Task GetOne_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();
        var id = Guid.NewGuid();

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetOne(id));
        Assert.Contains(id.ToString(), ex.Message);
    }

    // --- Remove ---

    [Fact]
    public async Task Remove_DeletesAndSaves_WhenModuleExists()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);

        await service.Remove(module.Id);

        Assert.Equal([module], moduleRepository.DeletedEntities);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_DoesNothing_WhenModuleDoesNotExist()
    {
        var (moduleRepository, _, service) = CreateService();

        await service.Remove(Guid.NewGuid());

        Assert.Empty(moduleRepository.DeletedEntities);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    // --- DetachResource ---

    [Fact]
    public async Task DetachResource_DetachesAndSaves_WhenModuleExists()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var resourceId = Guid.NewGuid();

        await service.DetachResource(module.Id, resourceId);

        Assert.Equal([(module.Id, resourceId)], moduleRepository.DetachResourceCalls);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DetachResource_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.DetachResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Update (ModuleForChangeDto) ---

    [Fact]
    public async Task Update_AppliesChangesAndSaves_WhenDataIsValid()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var data = ValidChangeDto();

        await service.Update(module.Id, data);

        Assert.Equal(data.Name, module.Name);
        Assert.Equal(data.Description, module.Description);
        Assert.Equal(data.Duration, module.Duration);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), ValidChangeDto()));
    }

    [Fact]
    public async Task Update_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(module.Id, data));

        Assert.Equal("Existing module", module.Name);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    // --- Update (JsonPatchDocument) ---

    [Fact]
    public async Task Update_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var patch = new JsonPatchDocument<ModuleForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await service.Update(module.Id, patch);

        Assert.Equal("Patched name", module.Name);
        Assert.Equal("Existing description", module.Description);
        Assert.Equal(1, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();
        var patch = new JsonPatchDocument<ModuleForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), patch));
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsValidationException_WhenPatchProducesInvalidData()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var patch = new JsonPatchDocument<ModuleForChangeDto>();
        patch.Replace(dto => dto.Name, "ab");

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(module.Id, patch));

        Assert.Equal("Existing module", module.Name);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    // --- UpdateResource (ResourceForChangeDto) ---

    [Fact]
    public async Task UpdateResource_AppliesChangesAndSaves_WhenResourceIsAttached()
    {
        var (moduleRepository, resourceRepository, service) = CreateService();
        var module = NewModule();
        var resource = NewResource();
        moduleRepository.TrackedModules.Add(module);
        moduleRepository.ResourcesByModuleId[module.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();

        await service.UpdateResource(module.Id, resource.Id, data);

        Assert.Equal(data.Name, resource.Name);
        Assert.Equal(data.Description, resource.Description);
        Assert.Equal(data.Type, resource.ResourceType);
        Assert.Equal(data.Data, resource.Data);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
        Assert.Equal(0, moduleRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenModuleDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenResourceIsNotAttachedToModule()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(module.Id, Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (moduleRepository, resourceRepository, service) = CreateService();
        var module = NewModule();
        var resource = NewResource();
        moduleRepository.TrackedModules.Add(module);
        moduleRepository.ResourcesByModuleId[module.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.UpdateResource(module.Id, resource.Id, data));

        Assert.Equal("Existing resource", resource.Name);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
    }

    // --- UpdateResource (JsonPatchDocument) ---

    [Fact]
    public async Task UpdateResource_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (moduleRepository, resourceRepository, service) = CreateService();
        var module = NewModule();
        var resource = NewResource();
        moduleRepository.TrackedModules.Add(module);
        moduleRepository.ResourcesByModuleId[module.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await service.UpdateResource(module.Id, resource.Id, patch);

        Assert.Equal("Patched resource name", resource.Name);
        Assert.Equal("Existing description", resource.Description);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_JsonPatch_ThrowsNotFoundException_WhenResourceIsNotAttachedToModule()
    {
        var (moduleRepository, _, service) = CreateService();
        var module = NewModule();
        moduleRepository.TrackedModules.Add(module);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(module.Id, Guid.NewGuid(), patch));
    }
}
