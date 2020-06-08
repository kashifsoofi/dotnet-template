namespace Template.Infrastructure.Tests.Integration.Testcontainers
{
    using DotNet.Testcontainers.Containers.Configurations;
    using DotNet.Testcontainers.Containers.Modules;

    public class MigrationsTestcontainer : TestcontainersContainer
    {
        protected MigrationsTestcontainer(ITestcontainersConfiguration configuration) : base(configuration)
        {
        }
    }
}