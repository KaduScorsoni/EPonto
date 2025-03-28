FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EPonto/EPonto.csproj", "EPonto/"]
RUN dotnet restore "./EPonto/EPonto.csproj"
COPY . .
WORKDIR "/src/EPonto"
RUN dotnet build "./EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls -al /app/publish  # Verifique os arquivos copiados para garantir que o EPonto.dll está presente
ENTRYPOINT ["dotnet", "EPonto.dll"]
