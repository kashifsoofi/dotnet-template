# webapi-service-template
This is `dotnet new` template for a ASP.NET Core Web API service.

## Usage
1. Clone repository locally  
2. Build nuget package  
`dotnet pack -o .`  
3. Install template for `dotnet new` using following command  
`dotnet new -i Personal.Utility.Templates.1.0.0.nupkg`  
4. Create new project using template  
`dotnet new webapi-service -s [ServiceName] -ag [AggregateName]`  
5. Add new aggregate to solution using  
`dotnet new aggregate -s [ServiceName] -ag [AggregateName]`  
5. Uninstall template using following command  
`dotnet new -u Personal.Utility.Templates`  

## References & Resources
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22  
* https://github.com/dotnet/dotnet-template-samples  
