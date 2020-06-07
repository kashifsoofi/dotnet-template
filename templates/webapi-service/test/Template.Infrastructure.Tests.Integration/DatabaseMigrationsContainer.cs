namespace Template.Infrastructure.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Docker.DotNet.Models;

    public class DatabaseMigrationsContainer : Container
    {
        private readonly string connectionString;

        public DatabaseMigrationsContainer(string imageName, string tag, string connectionString)
            : base(imageName, tag, Guid.NewGuid().ToString("N"))
        {
            this.connectionString = connectionString;
        }

        protected override Task<bool> IsReady()
        {
            return Task.FromResult(true);
        }

        public override HostConfig ToHostConfig()
        {
            return new HostConfig();
        }

        public override Config ToConfig()
        {
            var config = new Config();

            if (!string.IsNullOrEmpty(this.connectionString))
            {
                config.Cmd = new List<string> { "-cs", this.connectionString };
            }

            return config;
        }
    }
}
