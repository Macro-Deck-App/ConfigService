FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0.302-bullseye-slim-amd64 AS build
WORKDIR /src
COPY ["src/MacroDeck.ConfigService/MacroDeck.ConfigService.csproj", "src/MacroDeck.ConfigService/"]
COPY ["src/MacroDeck.ConfigService.Core/MacroDeck.ConfigService.Core.csproj", "src/MacroDeck.ConfigService.Core/"]
RUN dotnet restore "src/MacroDeck.ConfigService/MacroDeck.ConfigService.csproj"
COPY . .
WORKDIR "/src/src/MacroDeck.ConfigService"
RUN dotnet build "MacroDeck.ConfigService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MacroDeck.ConfigService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MacroDeck.ConfigService.dll"]
