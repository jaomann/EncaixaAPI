FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EncaixaAPI.csproj", "."]
RUN dotnet restore "EncaixaAPI.csproj"
COPY . .
RUN dotnet build "EncaixaAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "EncaixaAPI.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "/app/EncaixaAPI.dll"]