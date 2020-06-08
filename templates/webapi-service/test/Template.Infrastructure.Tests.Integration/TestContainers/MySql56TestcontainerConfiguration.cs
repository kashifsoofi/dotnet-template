namespace Template.Infrastructure.Tests.Integration.Testcontainers
{
    using DotNet.Testcontainers.Containers.Configurations.Abstractions;
    using DotNet.Testcontainers.Containers.WaitStrategies;

    public class MySql56TestcontainerConfiguration : TestcontainerDatabaseConfiguration
    {
        public MySql56TestcontainerConfiguration()
            : base("mysql:5.6", 3306)
        {
        }

        public override string Database
        {
            get => this.Environments["MYSQL_DATABASE"];
            set => this.Environments["MYSQL_DATABASE"] = value;
        }

        public override string Username
        {
            get => this.Environments["MYSQL_USER"];
            set => this.Environments["MYSQL_USER"] = value;
        }

        public override string Password
        {
            get => this.Environments["MYSQL_ROOT_PASSWORD"];
            set => this.Environments["MYSQL_ROOT_PASSWORD"] = value;
        }

        public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer()
            .UntilCommandIsCompleted($"mysql --host='localhost' --port='{this.DefaultPort}' --user='{this.Username}' --password='{this.Password}' --protocol=TCP --execute 'SHOW DATABASES;'");
    }
}