FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet tool install -g dotnet-script
RUN dotnet tool install -g dotnet-roundhouse --version 1.2.1

WORKDIR /db

# Copy all db files
COPY . .

ENTRYPOINT [ "dotnet-script", "Detector.csx", "--", "rh", "--silent", "-dt", "mysql" ]
CMD ["-cs", "server=db-server;database=template.db;uid=root;password=root_password;"]
