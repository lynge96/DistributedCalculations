FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Calculator.Api/Calculator.Api.csproj", "src/Calculator.Api/"]
COPY ["src/Calculator.Infrastructure/Calculator.Infrastructure.csproj", "src/Calculator.Infrastructure/"]
COPY ["src/Calculator.Application/Calculator.Application.csproj", "src/Calculator.Application/"]
COPY ["src/Shared.Domain/Shared.Domain.csproj", "src/Shared.Domain/"]
COPY ["src/Shared.RabbitMq/Shared.RabbitMq.csproj", "src/Shared.RabbitMq/"]
COPY ["src/Shared.Logging/Shared.Logging.csproj", "src/Shared.Logging/"]
RUN dotnet restore "src/Calculator.Api/Calculator.Api.csproj"
COPY . .
WORKDIR "/src/Calculator.Api"
RUN dotnet build "./Calculator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Calculator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calculator.Api.dll"]
