FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/CalculationHistory.Api/CalculationHistory.Api.csproj", "CalculationHistory.Api/"]
COPY ["src/Shared.Domain/Shared.Domain.csproj", "Shared.Domain/"]
COPY ["src/Shared.Logging/Shared.Logging.csproj", "Shared.Logging/"]
COPY ["src/Shared.RabbitMq/Shared.RabbitMq.csproj", "Shared.RabbitMq/"]

RUN dotnet restore "CalculationHistory.Api/CalculationHistory.Api.csproj"

COPY src/ ./

WORKDIR /src/CalculationHistory.Api
RUN dotnet build "CalculationHistory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CalculationHistory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculationHistory.Api.dll"]
