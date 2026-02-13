FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/History.Service.Tests/History.Service.Tests.csproj", "tests/History.Service.Tests/"]
RUN dotnet restore "tests/History.Service.Tests/History.Service.Tests.csproj"
COPY . .
WORKDIR "/src/tests/History.Service.Tests"
RUN dotnet build "./History.Service.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./History.Service.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "History.Service.Tests.dll"]
