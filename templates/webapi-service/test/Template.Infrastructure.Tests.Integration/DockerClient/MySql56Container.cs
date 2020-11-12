namespace Template.Infrastructure.Tests.Integration.DockerClient
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Docker.DotNet.Models;
    using MySqlConnector;

    public class MySql56Container : Container
    {
        public MySql56Container(string containerName)
            : base("mysql", "5.6", containerName)
        { }

        public string ConnectionString = "Server=127.0.0.1;Port=33060;Database=integrationdefaultdb;Uid=root;Pwd=integration123;SslMode=none;";

        protected override Task<bool> IsReady()
        {
            return Task.Run(() =>
            {
                try
                {
                    var connection = new MySqlConnection(this.ConnectionString);
                    connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {
                    return false;
                }
            });
        }

        public override HostConfig HostConfig()
        {
            return new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "3306/tcp", new List<PortBinding> { new PortBinding { HostPort = "33060" }}
                    },
                },
                PublishAllPorts = true,
            };
        }

        public override Config Config()
        {
            return new Config
            {
                Env = new List<string>
                {
                    "MYSQL_ROOT_PASSWORD=integration123",
                    "MYSQL_DATABASE=integrationdefaultdb",
                },
            };
        }
    }
}
