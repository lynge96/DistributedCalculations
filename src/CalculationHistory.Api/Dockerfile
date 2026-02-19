FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CalculationHistory.Api/CalculationHistory.Api.csproj", "src/CalculationHistory.Api/"]
COPY ["src/Shared.Domain/Shared.Domain.csproj", "src/Shared.Domain/"]
COPY ["src/Shared.Logging/Shared.Logging.csproj", "src/Shared.Logging/"]
COPY ["src/Shared.RabbitMq/Shared.RabbitMq.csproj", "src/Shared.RabbitMq/"]
RUN dotnet restore "src/CalculationHistory.Api/CalculationHistory.Api.csproj"
COPY . .
WORKDIR "/src/CalculationHistory.Api"
RUN dotnet build "./CalculationHistory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CalculationHistory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculationHistory.Api.dll"]
