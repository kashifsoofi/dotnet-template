FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
WORKDIR /build

# copy csproj and restore as distinct layers
COPY *.sln *.dcproj ./

# Copy the main source project files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Copy the test project files
COPY test/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p test/${file%.*}/ && mv $file test/${file%.*}/; done

# Restore to cache the layers
RUN dotnet restore Template.sln

# Copy all the source code and build
COPY ./src ./src
COPY ./test ./test

RUN dotnet build Template.sln -c Release --no-restore

# Run dotnet test on the solution
RUN dotnet test Template.sln -c Release --no-build --no-restore --verbosity=normal --filter Category!=AcceptanceTests

FROM build AS publish
RUN dotnet publish "./src/Template.MessageProcessor/Template.MessageProcessor.csproj" -c Release -o /app --no-restore

# App image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS runtime
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "Template.MessageProcessor.dll"]