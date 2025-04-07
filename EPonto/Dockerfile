# Etapa base para execu��o da aplica��o com ASP.NET Core Runtime 8.0.7
FROM mcr.microsoft.com/dotnet/aspnet:8.0.7 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build com .NET SDK 8.0.407
FROM mcr.microsoft.com/dotnet/sdk:8.0.407 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar apenas o csproj e restaurar depend�ncias
COPY ["EPonto/EPonto.csproj", "EPonto/"]
RUN dotnet restore "EPonto/EPonto.csproj"

# Copiar todo o c�digo-fonte
COPY . .

# Compilar o projeto
WORKDIR "/src/EPonto"
RUN dotnet build "EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar a aplica��o
RUN dotnet publish "EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:PublishTrimmed=false

# Etapa final de execu��o
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EPonto.dll"]
