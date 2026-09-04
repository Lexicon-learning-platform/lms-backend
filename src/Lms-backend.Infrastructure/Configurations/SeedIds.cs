namespace Lms_backend.Infrastructure.Configurations;

internal static class SeedIds
{
    public static readonly DateTime CreatedAt = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);

    public static class Users
    {
        public static readonly Guid Alex = Guid.Parse("44444444-0000-0000-0000-000000000001");
        public static readonly Guid Maria = Guid.Parse("44444444-0000-0000-0000-000000000002");
        public static readonly Guid Johan = Guid.Parse("44444444-0000-0000-0000-000000000003");
        public static readonly Guid Sara = Guid.Parse("44444444-0000-0000-0000-000000000004");
    }

    public static class Courses
    {
        public static readonly Guid FullStack = Guid.Parse("11111111-0000-0000-0000-000000000001");
        public static readonly Guid Backend = Guid.Parse("11111111-0000-0000-0000-000000000002");
        public static readonly Guid CloudDevOps = Guid.Parse("11111111-0000-0000-0000-000000000003");
    }

    public static class Modules
    {
        public static readonly Guid Git = Guid.Parse("22222222-0000-0000-0000-000000000001");
        public static readonly Guid Frontend = Guid.Parse("22222222-0000-0000-0000-000000000002");
        public static readonly Guid React = Guid.Parse("22222222-0000-0000-0000-000000000003");
        public static readonly Guid CSharp = Guid.Parse("22222222-0000-0000-0000-000000000004");
        public static readonly Guid AspNetCore = Guid.Parse("22222222-0000-0000-0000-000000000005");
        public static readonly Guid Docker = Guid.Parse("22222222-0000-0000-0000-000000000006");
        public static readonly Guid CiCd = Guid.Parse("22222222-0000-0000-0000-000000000007");
    }

    public static class Activities
    {
        public static readonly Guid IntroToGit = Guid.Parse("66666666-0000-0000-0000-000000000001");
        public static readonly Guid GitBranchingExercise = Guid.Parse("66666666-0000-0000-0000-000000000002");
        public static readonly Guid ReadProGitBook = Guid.Parse("66666666-0000-0000-0000-000000000003");
        public static readonly Guid CSharpSyntax = Guid.Parse("66666666-0000-0000-0000-000000000004");
        public static readonly Guid OopInCSharp = Guid.Parse("66666666-0000-0000-0000-000000000005");
        public static readonly Guid OopPracticeExercise = Guid.Parse("66666666-0000-0000-0000-000000000006");
        public static readonly Guid ConsoleAppAssignment = Guid.Parse("66666666-0000-0000-0000-000000000007");
        public static readonly Guid DockerFundamentals = Guid.Parse("66666666-0000-0000-0000-000000000008");
        public static readonly Guid DockerfileReview = Guid.Parse("66666666-0000-0000-0000-000000000009");
    }

    public static class Resources
    {
        public static readonly Guid ProGitBook = Guid.Parse("55555555-0000-0000-0000-000000000001");
        public static readonly Guid GitCheatSheet = Guid.Parse("55555555-0000-0000-0000-000000000002");
        public static readonly Guid MdnJavaScript = Guid.Parse("55555555-0000-0000-0000-000000000003");
        public static readonly Guid CSharpConventions = Guid.Parse("55555555-0000-0000-0000-000000000004");
        public static readonly Guid MsLearnAspNetCore = Guid.Parse("55555555-0000-0000-0000-000000000005");
        public static readonly Guid DockerDocs = Guid.Parse("55555555-0000-0000-0000-000000000006");
        public static readonly Guid OopPracticeInstructions = Guid.Parse("55555555-0000-0000-0000-000000000007");
        public static readonly Guid CourseSyllabusFullStack = Guid.Parse("55555555-0000-0000-0000-000000000008");
        public static readonly Guid MariaGitNotes = Guid.Parse("55555555-0000-0000-0000-000000000009");
        public static readonly Guid JohanConsoleAppTurnIn = Guid.Parse("55555555-0000-0000-0000-000000000010");
    }

    public static class CourseModules
    {
        public static readonly Guid FullStackGit = Guid.Parse("33333333-0000-0000-0000-000000000001");
        public static readonly Guid FullStackFrontend = Guid.Parse("33333333-0000-0000-0000-000000000002");
        public static readonly Guid FullStackReact = Guid.Parse("33333333-0000-0000-0000-000000000003");
        public static readonly Guid BackendGit = Guid.Parse("33333333-0000-0000-0000-000000000004");
        public static readonly Guid BackendCSharp = Guid.Parse("33333333-0000-0000-0000-000000000005");
        public static readonly Guid BackendAspNetCore = Guid.Parse("33333333-0000-0000-0000-000000000006");
        public static readonly Guid CloudDevOpsDocker = Guid.Parse("33333333-0000-0000-0000-000000000007");
        public static readonly Guid CloudDevOpsCiCd = Guid.Parse("33333333-0000-0000-0000-000000000008");
    }

    public static class ModuleResources
    {
        public static readonly Guid GitModuleProGitBook = Guid.Parse("88888888-0000-0000-0000-000000000001");
        public static readonly Guid FrontendModuleMdn = Guid.Parse("88888888-0000-0000-0000-000000000002");
        public static readonly Guid CSharpModuleConventions = Guid.Parse("88888888-0000-0000-0000-000000000003");
        public static readonly Guid AspNetCoreModuleMsLearn = Guid.Parse("88888888-0000-0000-0000-000000000004");
        public static readonly Guid DockerModuleDocs = Guid.Parse("88888888-0000-0000-0000-000000000005");
    }

    public static class ActivityResources
    {
        public static readonly Guid IntroToGitProGitBook = Guid.Parse("99999999-0000-0000-0000-000000000001");
        public static readonly Guid GitBranchingExerciseCheatSheet = Guid.Parse("99999999-0000-0000-0000-000000000002");
        public static readonly Guid OopPracticeExerciseInstructions = Guid.Parse("99999999-0000-0000-0000-000000000003");
        public static readonly Guid ConsoleAppAssignmentTurnIn = Guid.Parse("99999999-0000-0000-0000-000000000004");
    }

    public static class CourseResources
    {
        public static readonly Guid FullStackSyllabus = Guid.Parse("77777777-0000-0000-0000-000000000001");
    }

    public static class UserResources
    {
        public static readonly Guid MariaGitNotes = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    }
}
