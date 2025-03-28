# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto
COPY *.sln ./
COPY EPonto/*.csproj ./EPonto/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/

# Restaura os pacotes
RUN dotnet restore EPonto/EPonto.csproj

# Copia o restante do código
COPY . .

# Publica no modo Release
RUN dotnet publish EPonto/EPonto.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia o resultado da build
COPY --from=build /app/out .

# Verifica o conteúdo da pasta /app/out para garantir que o EPonto.dll está lá
RUN ls -l /app/out

# Define o comando de início
ENTRYPOINT ["dotnet", "EPonto.dll"]
