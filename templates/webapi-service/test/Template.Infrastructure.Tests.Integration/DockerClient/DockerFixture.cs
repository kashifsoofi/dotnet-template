namespace Template.Infrastructure.Tests.Integration.DockerClient
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Xunit;

    public class DockerFixture : IAsyncLifetime
    {
        private readonly IDockerClient dockerClient;

        private readonly Container databaseContainer;
        private readonly DatabaseMigrationsImage databaseMigrationsImage;
        private readonly Container databaseMigrationsContainer;

        public DockerFixture()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var endpoint = isWindows ? new Uri("npipe://./pipe/docker_engine") : new Uri("unix:///var/run/docker.sock");
            this.dockerClient = new DockerClientConfiguration(endpoint).CreateClient();
            this.databaseContainer = new MySql56Container("template-integration-db");
            this.databaseMigrationsImage = new DatabaseMigrationsImage("template-integration-db-migrations", "1.1", "../../../../../db/");
            this.databaseMigrationsContainer = new DatabaseMigrationsContainer("template-integration-db-migrations", "1.1", "");
        }

        public async Task InitializeAsync()
        {
            await this.databaseMigrationsImage.BuildAsync(this.dockerClient);
            await this.databaseContainer.StartAsync(this.dockerClient);
            await this.databaseMigrationsContainer.StartAsync(this.dockerClient);
        }

        public async Task DisposeAsync()
        {
            await this.databaseContainer.StopAsync(this.dockerClient);
            await this.databaseContainer.RemoveAsync(this.dockerClient);

            await this.databaseMigrationsContainer.StopAsync(this.dockerClient);
            await this.databaseMigrationsContainer.RemoveAsync(this.dockerClient);

            await this.databaseMigrationsImage.DeleteAsync(this.dockerClient);

            this.dockerClient.Dispose();
        }
    }
}