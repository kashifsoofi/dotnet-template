namespace Template.Infrastructure.Tests.Integration.Testcontainers
{
    using System;
    using System.Threading.Tasks;
    using DotNet.Testcontainers.Containers.Builders;
    using DotNet.Testcontainers.Containers.WaitStrategies;
    using DotNet.Testcontainers.Images.Builders;
    using Template.Infrastructure.Database;
    using Xunit;

    public class TestcontainersFixture : IAsyncLifetime
    {
        private MySql56Testcontainer mySqlTestcontainer;
        private string databaseMigrationsImage;
        private MigrationsTestcontainer migrationsTestcontainer;

        public IConnectionStringProvider ConnectionStringProvider { get; private set; }

        public TestcontainersFixture()
        {
        }

        public string Hostname => this.mySqlTestcontainer.Hostname;

        public int Port => this.mySqlTestcontainer.GetMappedPublicPort(3306);

        public string Password => "integration123";

        public async Task InitializeAsync()
        {
            try
            {
                var testcontainersBuilder = new TestcontainersBuilder<MySql56Testcontainer>()
                    .WithDatabase(new MySql56TestcontainerConfiguration
                    {
                        Database = "integrationdefaultdb",
                        Username = "root",
                        Password = this.Password,
                    });

                mySqlTestcontainer = testcontainersBuilder.Build();
                await mySqlTestcontainer.StartAsync();

                var databaseMigrationsImageBuilder = new ImageFromDockerfileBuilder()
                    .WithName("template-integration-db-migrations")
                    .WithDockerfile("Dockerfile")
                    .WithDockerfileDirectory("../../../../../db/")
                    .WithDeleteIfExists(true);
                this.databaseMigrationsImage = await databaseMigrationsImageBuilder.Build();

                var connectionString = mySqlTestcontainer.ConnectionString;
                connectionString = connectionString.Replace("localhost", "host.docker.internal");
                var databaseMigrationsContainerBuilder = new TestcontainersBuilder<MigrationsTestcontainer>()
                    .WithImage(databaseMigrationsImage)
                    .WithCommand($"-cs {connectionString}")
                    .WithWaitStrategy(Wait.ForUnixContainer());
                this.migrationsTestcontainer = databaseMigrationsContainerBuilder.Build();
                try
                {
                    await migrationsTestcontainer.StartAsync();
                    var exitCode = await migrationsTestcontainer.GetExitCode();
                    if (exitCode > 0)
                    {
                        throw new Exception("Database migrations failed");
                    }
                }
                catch
                {
                    throw;
                }

                this.ConnectionStringProvider = new ConnectionStringProvider(new DatabaseOptions
                {
                    Server = this.Hostname,
                    Port = this.Port,
                    Username = "root",
                    Password = this.Password,
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            if (this.mySqlTestcontainer != null)
            {
                await this.mySqlTestcontainer.DisposeAsync();
            }

            if (this.migrationsTestcontainer != null)
            {
                await this.migrationsTestcontainer.DisposeAsync();
            }
        }
    }
}