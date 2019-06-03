FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY SushiRunner/*.csproj ./SushiRunner/
COPY SushiRunner.Data/*.csproj ./SushiRunner.Data/
COPY SushiRunner.Services/*.csproj ./SushiRunner.Services/
COPY SushiRunner.Utilities/*.csproj ./SushiRunner.Utilities/
COPY SushiRunner.Tests/*.csproj ./SushiRunner.Tests/
RUN dotnet restore

# copy everything else and build app
COPY SushiRunner/. ./SushiRunner/
COPY SushiRunner.Data/. ./SushiRunner.Data/
COPY SushiRunner.Services/. ./SushiRunner.Services/
COPY SushiRunner.Utilities/. ./SushiRunner.Utilities/
COPY SushiRunner.Tests/. ./SushiRunner.Tests/
WORKDIR /app/SushiRunner
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/SushiRunner/out ./
ENTRYPOINT ["dotnet", "SushiRunner.dll"]