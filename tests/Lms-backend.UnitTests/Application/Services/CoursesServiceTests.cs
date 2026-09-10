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

public class CoursesServiceTests
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    private static Course NewCourse(
        Guid? id = null,
        string name = "Existing course",
        string description = "Existing description",
        DateOnly? startDate = null,
        int duration = 30) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name,
        Description = description,
        StartDate = startDate ?? Today.AddMonths(1),
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

    private static CourseForChangeDto ValidCreateDto(Guid[]? moduleIds = null) => new()
    {
        Name = "Web Development",
        Description = "Learn full-stack web development",
        StartDate = Today.AddMonths(1),
        Duration = 90,
        ModuleIds = moduleIds ?? [],
    };

    private static CourseForChangeDto ValidUpdateDto(Guid[]? moduleIds = null) => new()
    {
        Name = "Updated course",
        Description = "Updated description",
        StartDate = Today,
        Duration = 120,
        ModuleIds = moduleIds ?? [],
    };

    private static ResourceForChangeDto ValidResourceChangeDto() => new()
    {
        Name = "Docker cheat sheet",
        Description = "Covers Docker basics",
        Type = ResourceType.URL,
        Data = "https://example.com",
    };

    private static (FakeCourseRepository, FakeResourceRepository, FakeModuleRepository, CoursesService) CreateService()
    {
        var courseRepository = new FakeCourseRepository();
        var resourceRepository = new FakeResourceRepository();
        var moduleRepository = new FakeModuleRepository();
        var service = new CoursesService(courseRepository, resourceRepository, moduleRepository);
        return (courseRepository, resourceRepository, moduleRepository, service);
    }

    // --- GetByUserId ---

    [Fact]
    public async Task GetByUserId_ReturnsMappedDto_WhenCourseExists()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.CourseByUserId = course;

        var result = await service.GetByUserId(Guid.NewGuid());

        Assert.NotNull(result);
        Assert.Equal(course.Id, result!.Id);
    }

    [Fact]
    public async Task GetByUserId_ReturnsNull_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = await service.GetByUserId(Guid.NewGuid());

        Assert.Null(result);
    }

    // --- GetResources ---

    [Fact]
    public async Task GetResources_ReturnsMappedDtos_WhenCourseExists()
    {
        var (courseRepository, _, _, service) = CreateService();
        var resource = NewResource();
        var course = NewCourse();
        course.Resources.Add(new CourseResource { CourseId = course.Id, ResourceId = resource.Id, Resource = resource });
        courseRepository.ReadOnlyCourses.Add(course);

        var result = await service.GetResources(course.Id);

        var dto = Assert.Single(result);
        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResources_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResources(Guid.NewGuid()));
    }

    // --- GetResource ---

    [Fact]
    public async Task GetResource_ReturnsMappedDto_WhenResourceIsAttached()
    {
        var (courseRepository, _, _, service) = CreateService();
        var resource = NewResource();
        var course = NewCourse();
        course.Resources.Add(new CourseResource { CourseId = course.Id, ResourceId = resource.Id, Resource = resource });
        courseRepository.ReadOnlyCourses.Add(course);

        var dto = await service.GetResource(course.Id, resource.Id);

        Assert.Equal(resource.Id, dto.Id);
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task GetResource_ThrowsNotFoundException_WhenResourceIsNotAttached()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.ReadOnlyCourses.Add(course);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetResource(course.Id, Guid.NewGuid()));
    }

    // --- AddResource ---

    [Fact]
    public async Task AddResource_AddsAttachesSavesOnCourseRepository_AndReturnsRefetchedDto()
    {
        var (courseRepository, resourceRepository, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
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

        var result = await service.AddResource(course.Id, userId, data);

        var added = Assert.Single(resourceRepository.AddedEntities);
        Assert.Equal(userId, added.OwnerId);
        Assert.Equal(data.Name, added.Name);

        var (CourseId, ResourceId) = Assert.Single(courseRepository.AttachResourceCalls);
        Assert.Equal(course.Id, CourseId);
        Assert.Equal(added.Id, ResourceId);

        Assert.Equal(1, courseRepository.SaveChangesCallCount);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(userId, result.CreatedBy.Id);
    }

    [Fact]
    public async Task AddResource_ThrowsValidationException_AndDoesNotAddOrAttach_WhenDataIsInvalid()
    {
        var (courseRepository, resourceRepository, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.AddResource(course.Id, Guid.NewGuid(), data));

        Assert.Empty(resourceRepository.AddedEntities);
        Assert.Empty(courseRepository.AttachResourceCalls);
    }

    [Fact]
    public async Task AddResource_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AddResource(Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    // --- AttachResource ---

    [Fact]
    public async Task AttachResource_AttachesAndSaves_WhenNotAlreadyAttached()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        courseRepository.AttachResourceResult = true;
        var resourceId = Guid.NewGuid();

        var attached = await service.AttachResource(course.Id, resourceId);

        Assert.True(attached);
        Assert.Equal([(course.Id, resourceId)], courseRepository.AttachResourceCalls);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_DoesNotSave_WhenAlreadyAttached()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        courseRepository.AttachResourceResult = false;

        var attached = await service.AttachResource(course.Id, Guid.NewGuid());

        Assert.False(attached);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AttachResource_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.AttachResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Create ---

    [Fact]
    public async Task Create_AddsEntityWithModuleJoins_AndReturnsRefetchedDto_WhenValid()
    {
        var (courseRepository, _, moduleRepository, service) = CreateService();
        var moduleId = Guid.NewGuid();
        moduleRepository.MissingIds = [];
        var data = ValidCreateDto([moduleId]);

        var result = await service.Create(data);

        var added = Assert.Single(courseRepository.AddedEntities);
        Assert.Equal(data.Name, added.Name);
        Assert.Equal([moduleId], added.Modules.Select(m => m.ModuleId));
        Assert.Equal([moduleId], moduleRepository.LastGetMissingIdsCall);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
        Assert.Equal(data.Name, result.Name);
    }

    [Fact]
    public async Task Create_ThrowsValidationException_AndDoesNotAdd_WhenStartDateIsNotInTheFuture()
    {
        var (courseRepository, _, _, service) = CreateService();
        var data = ValidCreateDto();
        data.StartDate = Today;

        await Assert.ThrowsAsync<ValidationException>(() => service.Create(data));

        Assert.Empty(courseRepository.AddedEntities);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Create_ThrowsNotFoundException_AndDoesNotAdd_WhenModuleIdsAreMissing()
    {
        var (courseRepository, _, moduleRepository, service) = CreateService();
        var missingId = Guid.NewGuid();
        moduleRepository.MissingIds = [missingId];
        var data = ValidCreateDto([missingId]);

        await Assert.ThrowsAsync<NotFoundException>(() => service.Create(data));

        Assert.Empty(courseRepository.AddedEntities);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Create_SkipsModuleExistenceCheck_WhenModuleIdsIsEmpty()
    {
        var (_, _, moduleRepository, service) = CreateService();
        var data = ValidCreateDto([]);

        await service.Create(data);

        Assert.Null(moduleRepository.LastGetMissingIdsCall);
    }

    // --- GetMany ---

    [Fact]
    public async Task GetMany_UsesReadOnlyRepositoryAndReturnsMappedDtosWithPagination()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        var pagination = new PaginationMetadata(totalItemCount: 1, pageSize: 10, currentPage: 1);
        courseRepository.GetCoursesReadOnlyResult = ([course], pagination);
        var searchParams = new SearchParams(name: null, search: null);

        var (dtos, returnedPagination) = await service.GetMany(searchParams, page: 1, pageSize: 10);

        var dto = Assert.Single(dtos);
        Assert.Equal(course.Id, dto.Id);
        Assert.Same(pagination, returnedPagination);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetMany_DefaultsPageToOne_WhenPageIsNullOrLessThanOne(int? page)
    {
        var (courseRepository, _, _, service) = CreateService();

        await service.GetMany(new SearchParams(null, null), page: page, pageSize: 20);

        Assert.Equal(1, courseRepository.LastGetCoursesReadOnlyCall!.Value.Page);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPage_WhenValid()
    {
        var (courseRepository, _, _, service) = CreateService();

        await service.GetMany(new SearchParams(null, null), page: 3, pageSize: 20);

        Assert.Equal(3, courseRepository.LastGetCoursesReadOnlyCall!.Value.Page);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task GetMany_DefaultsPageSizeToTen_WhenPageSizeIsNullOrNotPositive(int? pageSize)
    {
        var (courseRepository, _, _, service) = CreateService();

        await service.GetMany(new SearchParams(null, null), page: 1, pageSize: pageSize);

        Assert.Equal(10, courseRepository.LastGetCoursesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_KeepsProvidedPageSize_WhenPositive()
    {
        var (courseRepository, _, _, service) = CreateService();

        await service.GetMany(new SearchParams(null, null), page: 1, pageSize: 1);

        Assert.Equal(1, courseRepository.LastGetCoursesReadOnlyCall!.Value.PageSize);
    }

    [Fact]
    public async Task GetMany_PassesSearchParamsThrough()
    {
        var (courseRepository, _, _, service) = CreateService();
        var searchParams = new SearchParams(name: "Docker", search: null);

        await service.GetMany(searchParams, page: 1, pageSize: 20);

        Assert.Same(searchParams, courseRepository.LastGetCoursesReadOnlyCall!.Value.SearchParams);
    }

    // --- GetOne ---

    [Fact]
    public async Task GetOne_ReturnsExtendedDto_WhenCourseExists()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.ReadOnlyCourses.Add(course);

        var result = await service.GetOne(course.Id);

        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Name, result.Name);
    }

    [Fact]
    public async Task GetOne_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();
        var id = Guid.NewGuid();

        var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetOne(id));
        Assert.Contains(id.ToString(), ex.Message);
    }

    // --- Remove ---

    [Fact]
    public async Task Remove_DeletesAndSaves_WhenCourseExists()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.ReadOnlyCourses.Add(course);

        await service.Remove(course.Id);

        Assert.Equal([course], courseRepository.DeletedEntities);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Remove_DoesNothing_WhenCourseDoesNotExist()
    {
        var (courseRepository, _, _, service) = CreateService();

        await service.Remove(Guid.NewGuid());

        Assert.Empty(courseRepository.DeletedEntities);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    // --- DetachResource ---

    [Fact]
    public async Task DetachResource_DetachesAndSaves_WhenCourseExists()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var resourceId = Guid.NewGuid();

        await service.DetachResource(course.Id, resourceId);

        Assert.Equal([(course.Id, resourceId)], courseRepository.DetachResourceCalls);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DetachResource_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.DetachResource(Guid.NewGuid(), Guid.NewGuid()));
    }

    // --- Update (CourseForChangeDto) ---

    [Fact]
    public async Task Update_AppliesChangesAndSaves_WhenValidAndNoModuleChanges()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var data = ValidUpdateDto();

        await service.Update(course.Id, data);

        Assert.Equal(data.Name, course.Name);
        Assert.Equal(data.Description, course.Description);
        Assert.Equal(data.StartDate, course.StartDate);
        Assert.Equal(data.Duration, course.Duration);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), ValidUpdateDto()));
    }

    [Fact]
    public async Task Update_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var data = ValidUpdateDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.Update(course.Id, data));

        Assert.Equal("Existing course", course.Name);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_ThrowsNotFoundException_AndDoesNotSave_WhenModuleIdsAreMissing()
    {
        var (courseRepository, _, moduleRepository, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var missingId = Guid.NewGuid();
        moduleRepository.MissingIds = [missingId];
        var data = ValidUpdateDto([missingId]);

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(course.Id, data));

        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_SyncsModules_AddsNewAndRemovesStale()
    {
        var (courseRepository, _, moduleRepository, service) = CreateService();
        var course = NewCourse();
        var staleModuleId = Guid.NewGuid();
        var newModuleId = Guid.NewGuid();
        course.Modules.Add(new CourseModule { CourseId = course.Id, ModuleId = staleModuleId });
        courseRepository.TrackedCourses.Add(course);
        moduleRepository.MissingIds = [];
        var data = ValidUpdateDto([newModuleId]);

        await service.Update(course.Id, data);

        Assert.Equal([newModuleId], course.Modules.Select(m => m.ModuleId));
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_SyncsModules_KeepsExistingJoinInstance_ForUnchangedModuleId()
    {
        var (courseRepository, _, moduleRepository, service) = CreateService();
        var course = NewCourse();
        var keptModuleId = Guid.NewGuid();
        var addedModuleId = Guid.NewGuid();
        var keptJoin = new CourseModule { CourseId = course.Id, ModuleId = keptModuleId, StartTimeOffset = 42 };
        course.Modules.Add(keptJoin);
        courseRepository.TrackedCourses.Add(course);
        moduleRepository.MissingIds = [];
        var data = ValidUpdateDto([keptModuleId, addedModuleId]);

        await service.Update(course.Id, data);

        Assert.Equal(2, course.Modules.Count);
        Assert.Same(keptJoin, course.Modules.Single(m => m.ModuleId == keptModuleId));
        Assert.Equal(42, course.Modules.Single(m => m.ModuleId == keptModuleId).StartTimeOffset);
    }

    // --- Update (JsonPatchDocument) ---

    [Fact]
    public async Task Update_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var patch = new JsonPatchDocument<CourseForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await service.Update(course.Id, patch);

        Assert.Equal("Patched name", course.Name);
        Assert.Equal("Existing description", course.Description);
        Assert.Equal(1, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task Update_JsonPatch_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();
        var patch = new JsonPatchDocument<CourseForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.Update(Guid.NewGuid(), patch));
    }

    // --- UpdateResource (ResourceForChangeDto) ---

    [Fact]
    public async Task UpdateResource_AppliesChangesAndSaves_WhenResourceIsAttached()
    {
        var (courseRepository, resourceRepository, _, service) = CreateService();
        var course = NewCourse();
        var resource = NewResource();
        courseRepository.TrackedCourses.Add(course);
        courseRepository.ResourcesByCourseId[course.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();

        await service.UpdateResource(course.Id, resource.Id, data);

        Assert.Equal(data.Name, resource.Name);
        Assert.Equal(data.Description, resource.Description);
        Assert.Equal(data.Type, resource.ResourceType);
        Assert.Equal(data.Data, resource.Data);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
        Assert.Equal(0, courseRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenCourseDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(Guid.NewGuid(), Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsNotFoundException_WhenResourceIsNotAttachedToCourse()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(course.Id, Guid.NewGuid(), ValidResourceChangeDto()));
    }

    [Fact]
    public async Task UpdateResource_ThrowsValidationException_AndDoesNotSave_WhenDataIsInvalid()
    {
        var (courseRepository, resourceRepository, _, service) = CreateService();
        var course = NewCourse();
        var resource = NewResource();
        courseRepository.TrackedCourses.Add(course);
        courseRepository.ResourcesByCourseId[course.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var data = ValidResourceChangeDto();
        data.Name = "ab";

        await Assert.ThrowsAsync<ValidationException>(() => service.UpdateResource(course.Id, resource.Id, data));

        Assert.Equal("Existing resource", resource.Name);
        Assert.Equal(0, resourceRepository.SaveChangesCallCount);
    }

    // --- UpdateResource (JsonPatchDocument) ---

    [Fact]
    public async Task UpdateResource_JsonPatch_AppliesPatchOnTopOfExistingValuesAndSaves()
    {
        var (courseRepository, resourceRepository, _, service) = CreateService();
        var course = NewCourse();
        var resource = NewResource();
        courseRepository.TrackedCourses.Add(course);
        courseRepository.ResourcesByCourseId[course.Id] = [resource];
        resourceRepository.TrackedResources.Add(resource);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await service.UpdateResource(course.Id, resource.Id, patch);

        Assert.Equal("Patched resource name", resource.Name);
        Assert.Equal("Existing description", resource.Description);
        Assert.Equal(1, resourceRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateResource_JsonPatch_ThrowsNotFoundException_WhenResourceIsNotAttachedToCourse()
    {
        var (courseRepository, _, _, service) = CreateService();
        var course = NewCourse();
        courseRepository.TrackedCourses.Add(course);
        var patch = new JsonPatchDocument<ResourceForChangeDto>();
        patch.Replace(dto => dto.Name, "Patched resource name");

        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateResource(course.Id, Guid.NewGuid(), patch));
    }
}
