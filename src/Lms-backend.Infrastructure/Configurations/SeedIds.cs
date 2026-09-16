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

        // FullStack course
        public static readonly Guid Emma = Guid.Parse("44444444-0000-0000-0000-000000000006");
        public static readonly Guid Oskar = Guid.Parse("44444444-0000-0000-0000-000000000007");
        public static readonly Guid Lina = Guid.Parse("44444444-0000-0000-0000-000000000008");
        public static readonly Guid Viktor = Guid.Parse("44444444-0000-0000-0000-000000000009");

        // Backend course
        public static readonly Guid Erik = Guid.Parse("44444444-0000-0000-0000-000000000010");
        public static readonly Guid Sofia = Guid.Parse("44444444-0000-0000-0000-000000000011");
        public static readonly Guid Anders = Guid.Parse("44444444-0000-0000-0000-000000000012");
        public static readonly Guid Elin = Guid.Parse("44444444-0000-0000-0000-000000000013");

        // CloudDevOps course
        public static readonly Guid Fredrik = Guid.Parse("44444444-0000-0000-0000-000000000014");
        public static readonly Guid Nina = Guid.Parse("44444444-0000-0000-0000-000000000015");
        public static readonly Guid Josefin = Guid.Parse("44444444-0000-0000-0000-000000000016");
        public static readonly Guid Martin = Guid.Parse("44444444-0000-0000-0000-000000000017");
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
        public static readonly Guid GitWorkflowAssignment = Guid.Parse("66666666-0000-0000-0000-000000000010");
        public static readonly Guid HtmlCssFundamentals = Guid.Parse("66666666-0000-0000-0000-000000000011");
        public static readonly Guid ResponsiveLayoutExercise = Guid.Parse("66666666-0000-0000-0000-000000000012");
        public static readonly Guid PortfolioPageAssignment = Guid.Parse("66666666-0000-0000-0000-000000000013");
        public static readonly Guid ReactComponentsProps = Guid.Parse("66666666-0000-0000-0000-000000000014");
        public static readonly Guid StateHooksExercise = Guid.Parse("66666666-0000-0000-0000-000000000015");
        public static readonly Guid TodoAppAssignment = Guid.Parse("66666666-0000-0000-0000-000000000016");
        public static readonly Guid BuildingRestApis = Guid.Parse("66666666-0000-0000-0000-000000000017");
        public static readonly Guid EfCoreMigrationsExercise = Guid.Parse("66666666-0000-0000-0000-000000000018");
        public static readonly Guid CrudApiAssignment = Guid.Parse("66666666-0000-0000-0000-000000000019");
        public static readonly Guid CustomDockerImageExercise = Guid.Parse("66666666-0000-0000-0000-000000000020");
        public static readonly Guid DockerizeAppAssignment = Guid.Parse("66666666-0000-0000-0000-000000000021");
        public static readonly Guid CiCdPipelineConcepts = Guid.Parse("66666666-0000-0000-0000-000000000022");
        public static readonly Guid GithubActionsExercise = Guid.Parse("66666666-0000-0000-0000-000000000023");
        public static readonly Guid DeploymentPipelineAssignment = Guid.Parse("66666666-0000-0000-0000-000000000024");
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

        // Git Workflow Assignment turn-ins
        public static readonly Guid GitWorkflowMariaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000011");
        public static readonly Guid GitWorkflowEmmaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000012");
        public static readonly Guid GitWorkflowOskarTurnin = Guid.Parse("55555555-0000-0000-0000-000000000013");
        public static readonly Guid GitWorkflowLinaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000014");
        public static readonly Guid GitWorkflowJohanTurnin = Guid.Parse("55555555-0000-0000-0000-000000000015");
        public static readonly Guid GitWorkflowErikTurnin = Guid.Parse("55555555-0000-0000-0000-000000000016");
        public static readonly Guid GitWorkflowSofiaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000017");
        public static readonly Guid GitWorkflowAndersTurnin = Guid.Parse("55555555-0000-0000-0000-000000000018");

        // Console App Assignment turn-ins (additional)
        public static readonly Guid ConsoleAppErikTurnin = Guid.Parse("55555555-0000-0000-0000-000000000019");
        public static readonly Guid ConsoleAppSofiaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000020");
        public static readonly Guid ConsoleAppAndersTurnin = Guid.Parse("55555555-0000-0000-0000-000000000021");

        // Portfolio Page Assignment turn-ins
        public static readonly Guid PortfolioMariaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000022");
        public static readonly Guid PortfolioEmmaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000023");
        public static readonly Guid PortfolioOskarTurnin = Guid.Parse("55555555-0000-0000-0000-000000000024");
        public static readonly Guid PortfolioLinaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000025");

        // Todo App Assignment turn-ins
        public static readonly Guid TodoAppMariaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000026");
        public static readonly Guid TodoAppEmmaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000027");
        public static readonly Guid TodoAppOskarTurnin = Guid.Parse("55555555-0000-0000-0000-000000000028");
        public static readonly Guid TodoAppViktorTurnin = Guid.Parse("55555555-0000-0000-0000-000000000029");

        // CRUD API Assignment turn-ins
        public static readonly Guid CrudApiJohanTurnin = Guid.Parse("55555555-0000-0000-0000-000000000030");
        public static readonly Guid CrudApiErikTurnin = Guid.Parse("55555555-0000-0000-0000-000000000031");
        public static readonly Guid CrudApiAndersTurnin = Guid.Parse("55555555-0000-0000-0000-000000000032");
        public static readonly Guid CrudApiElinTurnin = Guid.Parse("55555555-0000-0000-0000-000000000033");

        // Dockerize App Assignment turn-ins
        public static readonly Guid DockerizeSaraTurnin = Guid.Parse("55555555-0000-0000-0000-000000000034");
        public static readonly Guid DockerizeFredrikTurnin = Guid.Parse("55555555-0000-0000-0000-000000000035");
        public static readonly Guid DockerizeNinaTurnin = Guid.Parse("55555555-0000-0000-0000-000000000036");
        public static readonly Guid DockerizeJosefinTurnin = Guid.Parse("55555555-0000-0000-0000-000000000037");

        // Deployment Pipeline Assignment turn-ins
        public static readonly Guid DeployPipelineSaraTurnin = Guid.Parse("55555555-0000-0000-0000-000000000038");
        public static readonly Guid DeployPipelineFredrikTurnin = Guid.Parse("55555555-0000-0000-0000-000000000039");
        public static readonly Guid DeployPipelineJosefinTurnin = Guid.Parse("55555555-0000-0000-0000-000000000040");
        public static readonly Guid DeployPipelineMartinTurnin = Guid.Parse("55555555-0000-0000-0000-000000000041");
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

        public static readonly Guid GitWorkflowMariaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000005");
        public static readonly Guid GitWorkflowEmmaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000006");
        public static readonly Guid GitWorkflowOskarTurnin = Guid.Parse("99999999-0000-0000-0000-000000000007");
        public static readonly Guid GitWorkflowLinaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000008");
        public static readonly Guid GitWorkflowJohanTurnin = Guid.Parse("99999999-0000-0000-0000-000000000009");
        public static readonly Guid GitWorkflowErikTurnin = Guid.Parse("99999999-0000-0000-0000-000000000010");
        public static readonly Guid GitWorkflowSofiaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000011");
        public static readonly Guid GitWorkflowAndersTurnin = Guid.Parse("99999999-0000-0000-0000-000000000012");

        public static readonly Guid ConsoleAppErikTurnin = Guid.Parse("99999999-0000-0000-0000-000000000013");
        public static readonly Guid ConsoleAppSofiaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000014");
        public static readonly Guid ConsoleAppAndersTurnin = Guid.Parse("99999999-0000-0000-0000-000000000015");

        public static readonly Guid PortfolioMariaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000016");
        public static readonly Guid PortfolioEmmaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000017");
        public static readonly Guid PortfolioOskarTurnin = Guid.Parse("99999999-0000-0000-0000-000000000018");
        public static readonly Guid PortfolioLinaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000019");

        public static readonly Guid TodoAppMariaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000020");
        public static readonly Guid TodoAppEmmaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000021");
        public static readonly Guid TodoAppOskarTurnin = Guid.Parse("99999999-0000-0000-0000-000000000022");
        public static readonly Guid TodoAppViktorTurnin = Guid.Parse("99999999-0000-0000-0000-000000000023");

        public static readonly Guid CrudApiJohanTurnin = Guid.Parse("99999999-0000-0000-0000-000000000024");
        public static readonly Guid CrudApiErikTurnin = Guid.Parse("99999999-0000-0000-0000-000000000025");
        public static readonly Guid CrudApiAndersTurnin = Guid.Parse("99999999-0000-0000-0000-000000000026");
        public static readonly Guid CrudApiElinTurnin = Guid.Parse("99999999-0000-0000-0000-000000000027");

        public static readonly Guid DockerizeSaraTurnin = Guid.Parse("99999999-0000-0000-0000-000000000028");
        public static readonly Guid DockerizeFredrikTurnin = Guid.Parse("99999999-0000-0000-0000-000000000029");
        public static readonly Guid DockerizeNinaTurnin = Guid.Parse("99999999-0000-0000-0000-000000000030");
        public static readonly Guid DockerizeJosefinTurnin = Guid.Parse("99999999-0000-0000-0000-000000000031");

        public static readonly Guid DeployPipelineSaraTurnin = Guid.Parse("99999999-0000-0000-0000-000000000032");
        public static readonly Guid DeployPipelineFredrikTurnin = Guid.Parse("99999999-0000-0000-0000-000000000033");
        public static readonly Guid DeployPipelineJosefinTurnin = Guid.Parse("99999999-0000-0000-0000-000000000034");
        public static readonly Guid DeployPipelineMartinTurnin = Guid.Parse("99999999-0000-0000-0000-000000000035");
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
