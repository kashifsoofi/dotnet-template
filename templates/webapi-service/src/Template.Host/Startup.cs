namespace Template.Host
{
    using Autofac;
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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var databaseOptions = Configuration.GetSection("Database").Get<DatabaseOptions>();
            builder.RegisterInstance(databaseOptions).As<IDatabaseOptions>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>().SingleInstance();

            builder.RegisterType<AggregateNameAggregateFactory>().As<IAggregateNameAggregateFactory>().SingleInstance();
            builder.RegisterType<AggregateNameRepository>().As<IAggregateNameAggregateReadRepository>().SingleInstance();
            builder.RegisterType<AggregateNameRepository>().As<IAggregateNameAggregateWriteRepository>().SingleInstance();
        }
    }
}
