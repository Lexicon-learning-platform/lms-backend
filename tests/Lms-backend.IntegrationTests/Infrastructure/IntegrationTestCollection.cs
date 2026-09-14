namespace Lms_backend.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
    public const string Name = "Integration Tests";
}
