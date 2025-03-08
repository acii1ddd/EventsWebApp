FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY src/ .

RUN dotnet restore EventsApp.API/EventsApp.API.csproj
RUN dotnet publish EventsApp.API/EventsApp.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet","EventsApp.API.dll"]
