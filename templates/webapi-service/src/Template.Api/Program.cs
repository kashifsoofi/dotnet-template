namespace Template.Api
{
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using Serilog;
    using Serilog.Events;
    using Template.Contracts.Messages.Commands;

    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Template.Api.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            CreateHostBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Template.Api");

                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningCommandsAs(type => type.Namespace == "Template.Contracts.Messages.Commands");
                    conventions.DefiningEventsAs(type => type.Namespace == "Template.Contracts.Messages.Events");
                    conventions.DefiningMessagesAs(type => type.Namespace == "Template.Infrastructure.Messages.Responses");

                    endpointConfiguration.UsePersistence<LearningPersistence>();
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();

                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(CreateAggregateName), "Template.Host");
                    routing.RouteToEndpoint(typeof(UpdateAggregateName), "Template.Host");
                    routing.RouteToEndpoint(typeof(DeleteAggregateName), "Template.Host");

                    endpointConfiguration.MakeInstanceUniquelyAddressable("1");

                    endpointConfiguration.Recoverability()
                        .Delayed(x => x.NumberOfRetries(0))
                        .Immediate(x => x.NumberOfRetries(0));

                    return endpointConfiguration;
                })
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
