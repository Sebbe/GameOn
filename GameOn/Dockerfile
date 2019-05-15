FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch AS build
WORKDIR /src
COPY ["GameOn/GameOn.csproj", "GameOn/"]
RUN dotnet restore "GameOn/GameOn.csproj"
COPY . .
WORKDIR "/src/GameOn"
RUN dotnet build "GameOn.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GameOn.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GameOn.dll"]