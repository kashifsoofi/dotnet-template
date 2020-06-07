namespace Template.Infrastructure.Tests.Integration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Docker.DotNet.Models;
    using MySql.Data.MySqlClient;

    public class MySql56Container : Container
    {
        public MySql56Container(string containerName)
            : base("mysql", "5.6", containerName)
        { }

        public readonly string ConnectionString = "Server=127.0.0.1;Port=3306;Database=integrationdefaultdb;Uid=root;Pwd=integration123;";

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

        public override HostConfig ToHostConfig()
        {
            return new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "3306",
                        new List<PortBinding>
                        {
                            new PortBinding { HostPort = "3306", HostIP = "127.0.0.1" },
                        }
                    },
                },
            };
        }

        public override Config ToConfig()
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
