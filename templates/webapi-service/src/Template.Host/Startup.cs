namespace Template.Host
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Template.Domain.Aggregates.AggregateName;
    using Template.Infrastructure.AggregateRepositories.AggregateName;
    using Template.Infrastructure.Database;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseOptions = Configuration.GetSection("Database").Get<DatabaseOptions>();
            services.AddSingleton<IDatabaseOptions>(databaseOptions);
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

            services.AddSingleton<IAggregateNameAggregateFactory, AggregateNameAggregateFactory>();
            services.AddSingleton<IAggregateNameAggregateReadRepository, AggregateNameRepository>();
            services.AddSingleton<IAggregateNameAggregateWriteRepository, AggregateNameRepository>();
        }
    }
}
