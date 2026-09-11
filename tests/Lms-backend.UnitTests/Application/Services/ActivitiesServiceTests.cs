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

public class ActivitiesServiceTests
{
    private static readonly Guid ModuleId = Guid.NewGuid();

    private static Activity NewActivity(
        Guid? id = null,
        Guid? moduleId = null,
        string name = "Existing activity",
        string description = "Existing description",
        ActivityType type = ActivityType.Lecture,
        int startOffset = 0,
        int duration = 30) => new()
    {
        Id = id ?? Guid.NewGuid(),
        ModuleId = moduleId ?? ModuleId,
        Name = name,
        Description = description,
        ActivityType = type,
        StartTimeOffset = startOffset,
        DurationMinutes = duration,
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

    private static ActivityForChangeDto ValidChangeDto() => new()
    {
        Name = "Intro to Docker",
        Description = "Covers Docker basics",
        Type = ActivityType.Exercise,
        StartOffset = 60,
        Duration = 45,
    };

    private static ResourceForChangeDto ValidResourceChangeDto() => new()
    {
        Name = "Docker cheat sheet",
        Description = "Covers Docker basics",
        Type = ResourceType.URL,
        Data = "https://example.com",
    };

    private static (FakeActivityRepository, FakeResourceRepository, ActivitiesService) CreateService()
    {
        var activityRepository = new FakeActivityRepository();
        var resourceRepository = new FakeResourceRepository();
        var service = new ActivitiesService(activityRepository, resourceRepository);
        return (activityRepository, resourceRepository, service);
    }

    // --- GetResources ---

    [Fact]
    public async Task GetResources_ReturnsMappedDtos_WhenActivityExists()
    {
        var (activityRepository, _, service) = CreateService();
        var resource = NewResource();
        var activity = NewActivity();
        activity.Resources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id, Resource = resource });
        activityRepository.ReadOnlyActivities.Add(activity);

        var result = await service.GetResources(activity.ModuleId, activity.Id);

        var dto = Assert.Single(result);
        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResources_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResources(ModuleId, Guid.NewGuid()));
    }

    // --- GetResource ---

    [Fact]
    public async Task GetResource_ReturnsMappedDto_WhenResourceIsAttached()
    {
        var (activityRepository, _, service) = CreateService();
        var resource = NewResource();
        var activity = NewActivity();
        activity.Resources.Add(new ActivityResource { ActivityId = activity.Id, ResourceId = resource.Id, Resource = resource });
        activityRepository.ReadOnlyActivities.Add(activity);

        var dto = await service.GetResource(activity.ModuleId, activity.Id, resource.Id);

        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(ModuleId, Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenResourceIsNotAttached()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(activity.ModuleId, activity.Id, Guid.NewGuid()));
    }

    // --- AddResource ---

    [Fact]
    public async Task AddResource_AddsAttachesSavesOnActivityRepository_AndReturnsRefetchedDto()
    {
        var (activityRepository, resourceRepository, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
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

        var result = await service.AddResource(activity.ModuleId, activity.Id, userId, data);

        var added = Assert.Single(resourceRepository.AddedEntities);
        Assert.Equal(userId, added.OwnerId);
        Assert.Equal(data.Name, added.Name);

        var (ActivityId, ResourceId) = Assert.Single(activityRepository.AttachResourceCalls);
        Assert.Equal(activity.Id, ActivityId);
        Assert.Equal(added.Id, ResourceId);

        Assert.Equal(1, activityRepository.SaveChangesCallCount);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(userId, result.CreatedBy.Id);
    }

    [Fact]
    public async Task AddResource_ThrowsValidationException_AndDoesNotAddOrAttach_WhenDataIsInvalid()
    {
        var (activityRepository, resourceRepository, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.AddResource(activity.ModuleId, activity.Id, Guid.NewGuid(), data));

        Assert.Empty(resourceRepository.AddedEntities);
        Assert.Empty(activityRepository.AttachResourceCalls);
    }

    [Fact]
    public async Task AddResource_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AddResource(ModuleId, Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    // --- AttachResource ---

    [Fact]
    public async Task AttachResource_AttachesAndSaves_WhenNotAlreadyAttached()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
        activityRepository.AttachResourceResult = true;
        var resourceId = Guid.NewGuid();

        var attached = await service.AttachResource(activity.ModuleId, activity.Id, resourceId);

        Assert.True(attached);
        Assert.Equal([(activity.Id, resourceId)], activityRepository.AttachResourceCalls);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_DoesNotSave_WhenAlreadyAttached()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
        activityRepository.AttachResourceResult = false;

        var attached = await service.AttachResource(activity.ModuleId, activity.Id, Guid.NewGuid());

        Assert.False(attached);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AttachResource(ModuleId, Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Create ---

    [Fact]
    public async Task Create_AddsEntityAndReturnsMappedDto_WhenDataIsValidAndNoOverlap()
    {
        var (activityRepository, _, service) = CreateService();
        var data = ValidChangeDto();

        var result = await service.Create(ModuleId, Guid.NewGuid(), data);

        var added = Assert.Single(activityRepository.AddedEntities);
        Assert.Equal(ModuleId, added.ModuleId);
        Assert.Equal(data.Name, added.Name);
        Assert.Equal(data.Type, added.ActivityType);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_AndDoesNotCheckOverlap_WhenDataIsInvalid()
    {
        var (activityRepository, _, service) = CreateService();
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(ModuleId, Guid.NewGuid(), data));

        Assert.Empty(activityRepository.AddedEntities);
        Assert.Null(activityRepository.LastHasOverlappingActivityCall);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_AndDoesNotAdd_WhenActivityOverlaps()
    {
        var (activityRepository, _, service) = CreateService();
        activityRepository.HasOverlappingActivityResult = true;

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(ModuleId, Guid.NewGuid(), ValidChangeDto()));

        Assert.Empty(activityRepository.AddedEntities);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    // --- GetMany ---

    [Fact]
    public async Task GetMany_UsesReadOnlyRepositoryAndReturnsMappedDtosWithPagination()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        var pagination = new PaginationMetadata(totalItemCount: 1, pageSize: 10, currentPage: 1);
        activityRepository.GetActivitiesReadOnlyResult = ([activity], pagination);
        var searchParams = new ActivitySearchParams(name: null, search: null, type: null);

        var (dtos, returnedPagination) = await service.GetMany(ModuleId, searchParams, page: 1, pageSize: 10);

        var dto = Assert.Single(dtos);
        Assert.Equal(activity.Id, dto.Id);
        Assert.Same(pagination, returnedPagination);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetMany_DefaultsPageToOne_WhenPageIsNullOrLessThanOne(int? page)
    {
        var (activityRepository, _, service) = CreateService();

        await service.GetMany(ModuleId, new ActivitySearchParams(null, null, null), page: page, pageSize: 20);

        Assert.Equal(1, activityRepository.LastGetActivitiesReadOnlyCall!.Value.Page);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPage_WhenValid()
    {
        var (activityRepository, _, service) = CreateService();

        await service.GetMany(ModuleId, new ActivitySearchParams(null, null, null), page: 3, pageSize: 20);

        Assert.Equal(3, activityRepository.LastGetActivitiesReadOnlyCall!.Value.Page);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task GetMany_DefaultsPageSizeToTen_WhenPageSizeIsNullOrNotPositive(int? pageSize)
    {
        var (activityRepository, _, service) = CreateService();

        await service.GetMany(ModuleId, new ActivitySearchParams(null, null, null), page: 1, pageSize: pageSize);

        Assert.Equal(10, activityRepository.LastGetActivitiesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPageSize_WhenPositive()
    {
        var (activityRepository, _, service) = CreateService();

        await service.GetMany(ModuleId, new ActivitySearchParams(null, null, null), page: 1, pageSize: 1);

        Assert.Equal(1, activityRepository.LastGetActivitiesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_PassesModuleIdAndSearchParamsThrough()
    {
        var (activityRepository, _, service) = CreateService();
        var searchParams = new ActivitySearchParams(name: "Docker", search: null, type: ActivityType.Lecture);

        await service.GetMany(ModuleId, searchParams, page: 1, pageSize: 20);

        Assert.Equal(ModuleId, activityRepository.LastGetActivitiesReadOnlyCall!.Value.ModuleId);
        Assert.Same(searchParams, activityRepository.LastGetActivitiesReadOnlyCall!.Value.SearchParams);
    }

    // --- GetOne ---

    [Fact]
    public async Task GetOne_ReturnsExtendedDto_WhenActivityExists()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);

        var result = await service.GetOne(activity.ModuleId, activity.Id);

        Assert.Equal(activity.Id, result.Id);
        Assert.Equal(activity.Name, result.Name);
    }

    [Fact]
    public async Task GetOne_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();
        var id = Guid.NewGuid();

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetOne(ModuleId, id));
        Assert.Contains(id.ToString(), ex.Message);
    }

    [Fact]
    public async Task GetOne_ThrowsNotFoundException_WhenModuleIdDoesNotMatch()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetOne(Guid.NewGuid(), activity.Id));
    }

    // --- Remove ---

    [Fact]
    public async Task Remove_DeletesAndSaves_WhenActivityExists()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);

        await service.Remove(activity.ModuleId, activity.Id);

        Assert.Equal([activity], activityRepository.DeletedEntities);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_DoesNothing_WhenActivityDoesNotExist()
    {
        var (activityRepository, _, service) = CreateService();

        await service.Remove(ModuleId, Guid.NewGuid());

        Assert.Empty(activityRepository.DeletedEntities);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    // --- DetachResource ---

    [Fact]
    public async Task DetachResource_DetachesAndSaves_WhenActivityExists()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
        var resourceId = Guid.NewGuid();

        await service.DetachResource(activity.ModuleId, activity.Id, resourceId);

        Assert.Equal([(activity.Id, resourceId)], activityRepository.DetachResourceCalls);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DetachResource_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.DetachResource(ModuleId, Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Update (ActivityForChangeDto) ---

    [Fact]
    public async Task Update_AppliesChangesAndSaves_WhenValidAndNoOverlap()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);
        var data = ValidChangeDto();

        await service.Update(activity.ModuleId, activity.Id, data);

        Assert.Equal(data.Name, activity.Name);
        Assert.Equal(data.Description, activity.Description);
        Assert.Equal(data.Type, activity.ActivityType);
        Assert.Equal(data.StartOffset, activity.StartTimeOffset);
        Assert.Equal(data.Duration, activity.DurationMinutes);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(ModuleId, Guid.NewGuid(), ValidChangeDto()));
    }

    [Fact]
    public async Task Update_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);
        var data = ValidChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(activity.ModuleId, activity.Id, data));

        Assert.Equal("Existing activity", activity.Name);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsValidationException_AndExcludesSelf_WhenActivityOverlaps()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);
        activityRepository.HasOverlappingActivityResult = true;

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(activity.ModuleId, activity.Id, ValidChangeDto()));

        Assert.Equal(activity.Id, activityRepository.LastHasOverlappingActivityCall!.Value.ExcludeId);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    // --- Update (JsonPatchDocument) ---

    [Fact]
    public async Task Update_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);
        var patch = new JsonPatchDocument<ActivityForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await service.Update(activity.ModuleId, activity.Id, patch);

        Assert.Equal("Patched name", activity.Name);
        Assert.Equal("Existing description", activity.Description);
        Assert.Equal(1, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();
        var patch = new JsonPatchDocument<ActivityForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(ModuleId, Guid.NewGuid(), patch));
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsValidationException_WhenPatchProducesInvalidData()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.ReadOnlyActivities.Add(activity);
        var patch = new JsonPatchDocument<ActivityForChangeDto>();
        patch.Replace(dto => dto.Name, "ab");

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(activity.ModuleId, activity.Id, patch));

        Assert.Equal("Existing activity", activity.Name);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    // --- UpdateResource (ResourceForChangeDto) ---

    [Fact]
    public async Task UpdateResource_AppliesChangesAndSaves_WhenResourceIsAttached()
    {
        var (activityRepository, resourceRepository, service) = CreateService();
        var activity = NewActivity();
        var resource = NewResource();
        activityRepository.TrackedActivities.Add(activity);
        activityRepository.ResourcesByActivityId[activity.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();

        await service.UpdateResource(activity.ModuleId, activity.Id, resource.Id, data);

        Assert.Equal(data.Name, resource.Name);
        Assert.Equal(data.Description, resource.Description);
        Assert.Equal(data.Type, resource.ResourceType);
        Assert.Equal(data.Data, resource.Data);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
        Assert.Equal(0, activityRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenActivityDoesNotExist()
    {
        var (_, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(ModuleId, Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenResourceIsNotAttachedToActivity()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(activity.ModuleId, activity.Id, Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (activityRepository, resourceRepository, service) = CreateService();
        var activity = NewActivity();
        var resource = NewResource();
        activityRepository.TrackedActivities.Add(activity);
        activityRepository.ResourcesByActivityId[activity.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.UpdateResource(activity.ModuleId, activity.Id, resource.Id, data));

        Assert.Equal("Existing resource", resource.Name);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
    }

    // --- UpdateResource (JsonPatchDocument) ---

    [Fact]
    public async Task UpdateResource_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (activityRepository, resourceRepository, service) = CreateService();
        var activity = NewActivity();
        var resource = NewResource();
        activityRepository.TrackedActivities.Add(activity);
        activityRepository.ResourcesByActivityId[activity.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await service.UpdateResource(activity.ModuleId, activity.Id, resource.Id, patch);

        Assert.Equal("Patched resource name", resource.Name);
        Assert.Equal("Existing description", resource.Description);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_JsonPatch_ThrowsNotFoundException_WhenResourceIsNotAttachedToActivity()
    {
        var (activityRepository, _, service) = CreateService();
        var activity = NewActivity();
        activityRepository.TrackedActivities.Add(activity);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(activity.ModuleId, activity.Id, Guid.NewGuid(), patch));
    }
}
