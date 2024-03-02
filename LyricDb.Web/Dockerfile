ARG GITHUB_TOKEN
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LyricDb.Web.csproj", "LyricDb.Web/"]
RUN dotnet nuget add source https://nuget.pkg.github.com/kengwang/index.json -n github -u kengwang -p $GITHUB_TOKEN --store-password-in-clear-text
RUN dotnet restore "LyricDb.Web/LyricDb.Web.csproj"
COPY [".", "LyricDb.Web/"]
WORKDIR "/src/LyricDb.Web"
RUN dotnet build "LyricDb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LyricDb.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LyricDb.Web.dll"]
