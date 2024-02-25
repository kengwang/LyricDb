FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LyricDb.Worker/LyricDb.Worker.csproj", "LyricDb.Worker/"]
RUN dotnet restore "LyricDb.Worker/LyricDb.Worker.csproj"
COPY . .
WORKDIR "/src/LyricDb.Worker"
RUN dotnet build "LyricDb.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LyricDb.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LyricDb.Worker.dll"]
