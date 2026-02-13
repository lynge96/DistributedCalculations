FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Calculator.Api.Tests/Calculator.Api.Tests.csproj", "tests/Calculator.Api.Tests/"]
RUN dotnet restore "tests/Calculator.Api.Tests/Calculator.Api.Tests.csproj"
COPY . .
WORKDIR "/src/tests/Calculator.Api.Tests"
RUN dotnet build "./Calculator.Api.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Calculator.Api.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calculator.Api.Tests.dll"]
