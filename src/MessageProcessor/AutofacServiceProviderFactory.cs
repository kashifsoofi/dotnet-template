using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace MessageProcessor
{
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return new ContainerBuilder();
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
