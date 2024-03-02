ARG GITHUB_TOKEN
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . "LyricDb.Worker"
RUN dotnet nuget add source https://nuget.pkg.github.com/kengwang/index.json -n github -u kengwang -p $GITHUB_TOKEN --store-password-in-clear-text
RUN dotnet restore "LyricDb.Worker/LyricDb.Worker.csproj"
WORKDIR "/src/LyricDb.Worker"
RUN dotnet build "LyricDb.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LyricDb.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LyricDb.Worker.dll"]
