using System;
using System.Threading.Tasks;
using Docker.DotNet;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.WaitStrategies;
using DotNet.Testcontainers.Images.Builders;
using MySql.Data.MySqlClient;
using Template.Infrastructure.Database;
using Xunit;

namespace Template.Infrastructure.Tests.Integration
{
    using System.Runtime.InteropServices;

    public class DatabaseFixture : IAsyncLifetime
    {
        private const string Database = "integrationdefaultdb";
        private const string Password = "integration123";

        //private readonly TestcontainersContainer databaseContainer;
        //private TestcontainersContainer databaseMigrationsContainer;

        private readonly IDockerClient dockerClient;

        private readonly Container databaseContainer;
        private readonly DatabaseMigrationsImage databaseMigrationsImage;
        private readonly Container databaseMigrationsContainer;

        //private string databaseMigrationsImage;

        public IConnectionStringProvider ConnectionStringProvider { get; private set; }

        public DatabaseFixture()
        {
            //var databaseContainerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
            //    .WithName("template-integration-db")
            //    .WithImage("mysql:5.6")
            //    .WithEnvironment("MYSQL_ROOT_PASSWORD", Password)
            //    .WithEnvironment("MYSQL_DATABASE", Database)
            //    .WithPortBinding(3306, true)
            //    .WithWaitStrategy(Wait.ForUnixContainer());

            //this.databaseContainer = databaseContainerBuilder.Build();
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

            this.ConnectionStringProvider = new ConnectionStringProvider(new DatabaseOptions
            {
                Server = "localhost",
                Port = 33060,
                Username = "root",
                Password = Password,
            });

            await this.databaseMigrationsContainer.StartAsync(this.dockerClient);

            //var databaseMigrationsImageBuilder = new ImageFromDockerfileBuilder()
            //    .WithName("template-integration-db-migrations")
            //    .WithDockerfile("Dockerfile")
            //    .WithDockerfileDirectory("../../../../../db/")
            //    .WithDeleteIfExists(true);

            //this.databaseMigrationsImage = await databaseMigrationsImageBuilder.Build();

            //await databaseContainer.StartAsync();

            //this.ConnectionStringProvider = new ConnectionStringProvider(new DatabaseOptions
            //{
            //    Server = this.databaseContainer.Hostname,
            //    Port = this.databaseContainer.GetMappedPublicPort(3306),
            //    Database = Database,
            //    Username = "root",
            //    Password = Password,
            //});

            //var isReady = await WaitUntil(() => ConnectionIsReady(this.ConnectionStringProvider.TemplateConnectionString), 250);
            //if (!isReady)
            //{
            //    throw new TimeoutException();
            //}

            //var databaseMigrationsContainerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
            //    .WithImage(databaseMigrationsImage)
            //    .WithCommand($"-cs {this.ConnectionStringProvider.TemplateConnectionString}")
            //    .WithWaitStrategy(Wait.ForUnixContainer());
            //this.databaseMigrationsContainer = databaseMigrationsContainerBuilder.Build();
            //try
            //{
            //    await databaseMigrationsContainer.StartAsync();
            //    var exitCode = await databaseContainer.GetExitCode();
            //    if (exitCode > 0)
            //    {
            //        throw new Exception("Database migrations failed");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task DisposeAsync()
        {
            await this.databaseContainer.StopAsync(this.dockerClient);
            await this.databaseContainer.RemoveAsync(this.dockerClient);

            await this.databaseMigrationsContainer.StopAsync(this.dockerClient);
            await this.databaseMigrationsContainer.RemoveAsync(this.dockerClient);

            //await this.databaseContainer.DisposeAsync();
            //await this.databaseMigrationsContainer.DisposeAsync();
        }

        private async Task<bool> WaitUntil(Func<Task<bool>> wait, int frequency, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!await wait())
                {
                    await Task.Delay(frequency);
                };
            });

            var completedTask = await Task.WhenAny(waitTask, Task.Delay(timeout));
            return completedTask == waitTask;
        }

        private Task<bool> ConnectionIsReady(string connectionString)
        {
            return Task.Run(() =>
            {
                try
                {
                    var connection = new MySqlConnection(connectionString);
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
