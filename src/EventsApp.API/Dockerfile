FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EventsApp.API/EventsApp.API.csproj", "src/EventsApp.API/"]
COPY ["src/EventsApp.BLL/EventsApp.BLL.csproj", "src/EventsApp.BLL/"]
COPY ["src/EventsApp.DAL.Postgres/EventsApp.DAL.Postgres.csproj", "src/EventsApp.DAL.Postgres/"]
COPY ["src/EventsApp.Domain/EventsApp.Domain.csproj", "src/EventsApp.Domain/"]
COPY ["src/EventsApp.Configuration/EventsApp.Configuration.csproj", "src/EventsApp.Configuration/"]
RUN dotnet restore "src/EventsApp.API/EventsApp.API.csproj"
COPY . .
WORKDIR "/src/src/EventsApp.API"
RUN dotnet build "EventsApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventsApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsApp.API.dll"]
