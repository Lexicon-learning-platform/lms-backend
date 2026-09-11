using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure;
using Lms_backend.UnitTests.TestHelpers;

namespace Lms_backend.UnitTests.Infrastructure;

public class UpdatedAtInterceptorTests
{
    private static readonly DateTime OriginalUpdatedAt = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static Course CreateCourse() => new()
    {
        Id = Guid.NewGuid(),
        CreatedAt = OriginalUpdatedAt,
        UpdatedAt = OriginalUpdatedAt,
        Name = "Test course",
        Description = "Test description",
        StartDate = DateOnly.FromDateTime(OriginalUpdatedAt),
        Duration = 4,
    };

    [Fact]
    public void SaveChanges_SetsUpdatedAt_OnModifiedEntity()
    {
        using var context = TestDbContextFactory.Create(new UpdatedAtInterceptor());
        var course = CreateCourse();
        context.Courses.Add(course);
        context.SaveChanges();

        course.Name = "Updated name";
        context.SaveChanges();

        Assert.True(course.UpdatedAt > OriginalUpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsUpdatedAt_OnModifiedEntity()
    {
        await using var context = TestDbContextFactory.Create(new UpdatedAtInterceptor());
        var course = CreateCourse();
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        course.Name = "Updated name";
        await context.SaveChangesAsync();

        Assert.True(course.UpdatedAt > OriginalUpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_PersistsUpdatedUpdatedAt_ToTheStore()
    {
        var databaseName = Guid.NewGuid().ToString();
        var course = CreateCourse();

        await using (var context = TestDbContextFactory.Create(databaseName, new UpdatedAtInterceptor()))
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            course.Name = "Updated name";
            await context.SaveChangesAsync();
        }

        // Read through a separate, untracked context so this proves the value reached the
        // store, rather than just reading back the same tracked in-memory entity instance.
        await using var readContext = TestDbContextFactory.Create(databaseName);
        var persisted = await readContext.Courses.FindAsync(course.Id);

        Assert.NotNull(persisted);
        Assert.True(persisted!.UpdatedAt > OriginalUpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_DoesNotSetUpdatedAt_OnNewlyAddedEntity()
    {
        await using var context = TestDbContextFactory.Create(new UpdatedAtInterceptor());
        var course = CreateCourse();
        context.Courses.Add(course);

        await context.SaveChangesAsync();

        Assert.Equal(OriginalUpdatedAt, course.UpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_DoesNotThrow_WhenThereAreNoPendingChanges()
    {
        await using var context = TestDbContextFactory.Create(new UpdatedAtInterceptor());

        var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.Null(exception);
    }
}
