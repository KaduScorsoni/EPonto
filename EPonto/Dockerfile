# Use .NET 8.0 ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Baixar e instalar o SDK .NET 8.0.407
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Baixar e instalar o SDK 8.0.407 se necessário
RUN wget https://download.visualstudio.microsoft.com/download/pr/54d7cbdd-e278-4a27-909f-9d19ee5e6b1c/51fe8d70811654843fdb37b4b3439f76/dotnet-sdk-8.0.407-linux-x64.tar.gz -O /tmp/dotnet-sdk.tar.gz
RUN mkdir -p /usr/share/dotnet && tar -xvf /tmp/dotnet-sdk.tar.gz -C /usr/share/dotnet
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Restaurar as dependências do projeto
COPY ["EPonto/EPonto.csproj", "EPonto/"]
RUN dotnet restore "./EPonto/EPonto.csproj"

# Copiar o código-fonte para o container e compilar
COPY . . 
WORKDIR "/src/EPonto"
RUN dotnet build "./EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EPonto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:PublishTrimmed=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPonto.dll"]
