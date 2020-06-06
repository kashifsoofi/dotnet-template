using System;
namespace Template.Infrastructure.Database
{
    public interface IConnectionStringProvider
    {
        string TemplateConnectionString { get; }
    }
}
