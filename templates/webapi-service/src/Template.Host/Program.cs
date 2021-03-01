namespace Template.Host
{
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using Serilog;
    using Serilog.Events;

    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Template.Api.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var host = CreateHostBuilder(args)
                .UseSerilog()
                .ConfigureContainer((HostBuilderContext hostBuilderContext, ContainerBuilder builder) =>
                {
                    var startup = new Startup(hostBuilderContext.Configuration);
                    startup.ConfigureContainer(builder);
                })
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Template.Host");

                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningCommandsAs(type => type.Namespace == "Template.Contracts.Messages.Commands");
                    conventions.DefiningEventsAs(type => type.Namespace == "Template.Contracts.Messages.Events");
                    conventions.DefiningMessagesAs(type => type.Namespace == "Template.Infrastructure.Messages.Responses");

                    endpointConfiguration.UsePersistence<LearningPersistence>();
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();

                    var routing = transport.Routing();

                    endpointConfiguration.Recoverability()
                        .Delayed(x => x.NumberOfRetries(0))
                        .Immediate(x => x.NumberOfRetries(0));

                    return endpointConfiguration;
                })
                .UseConsoleLifetime();

            await host.RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
