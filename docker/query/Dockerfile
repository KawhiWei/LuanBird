FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Shine.Query/Shine.Query.csproj", "src/Shine.Query/"]
COPY ["src/Shine.Dto/Shine.Dto.csproj", "src/Shine.Dto/"]
COPY ["src/Shine.Domain.Shared/Shine.Domain.Shared.csproj", "src/Shine.Domain.Shared/"]
COPY ["src/Shine.Domain/Shine.Domain.csproj", "src/Shine.Domain/"]
COPY ["src/Shine.Infrastructure/Shine.Infrastructure.csproj", "src/Shine.Infrastructure/"]
COPY ["src/Shine.Persistence/Shine.Persistence.csproj", "src/Shine.Persistence/"]
RUN dotnet restore "src/Shine.Query/Shine.Query.csproj"
COPY . .
WORKDIR "/src/src/Shine.Query"
RUN dotnet build "Shine.Query.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Shine.Query.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shine.Query.dll"]
