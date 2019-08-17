# webapi-service-template
This is `dotnet new` template for a ASP.NET Core Web API service.

## Usage
1. Clone repository locally
2. Install template for `dotnet new` using following command  
`dotnet new -i [Absolute path to repository]/webapi-service-template`
3. Create new project using template  
`dotnet new -s [ServiceName]`
4. Uninstall template using following command  
`dotnet new -u [Absolute path to repository]/webapi-service-template`

## Initialize DB
1. build docker image `docker build . -t webapi-service:db`
2. run docker image to run migrations on MySql server exposed on localhost
`docker run webapi-service:db -cs "Server=host.docker.internal; Database=webapiservice; Uid=<uid>; Pwd=<pwd>;"`

## References & Resources
* https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22
* https://github.com/dotnet/dotnet-template-samples
