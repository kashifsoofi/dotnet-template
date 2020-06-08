namespace Template.Infrastructure.Tests.Integration.Testcontainers
{
    using DotNet.Testcontainers.Containers.Configurations;
    using DotNet.Testcontainers.Containers.Modules.Abstractions;

    public class MySql56Testcontainer : TestcontainerDatabase
    {
        internal MySql56Testcontainer(ITestcontainersConfiguration configuration) : base(configuration)
        {
        }

        public override string ConnectionString => $"Server={this.Hostname};Port={this.Port};Database={this.Database};Uid={this.Username};Pwd={this.Password};";
    }
}