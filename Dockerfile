# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar únicamente el archivo .csproj
COPY API_GestionEconomia.csproj ./

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código fuente (solo lo necesario)
COPY . ./

# Publicar la aplicacion
RUN dotnet publish API_GestionEconomia.csproj -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-env
WORKDIR /app
COPY --from=build-env /app/out .

# Definir en qué puerto escuchará la app
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

ENTRYPOINT ["dotnet", "API_GestionEconomia.dll"]
