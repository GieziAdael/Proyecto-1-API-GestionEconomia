# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el proyecto y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/out ./

# Variables de entorno para la cadena de conexión
ENV DB_SERVER=""
ENV DB_NAME=""
ENV DB_USER=""
ENV DB_PASS=""

# Exponer puerto 80
EXPOSE 80

# Ejecutar la Web API
ENTRYPOINT ["dotnet", "API_GestionEconomia.dll"]
