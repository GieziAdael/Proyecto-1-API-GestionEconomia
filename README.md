🧩 Proyecto 1 - API Gestión de Economía

Este proyecto es una Web API desarrollada con .NET orientada a la gestión económica de usuarios y organizaciones,
permitiendo registrar, consultar y administrar movimientos financieros, así como los roles de los organizadores asociados.

La API está diseñada bajo una arquitectura RESTful y utiliza Entity Framework Core para la conexión con una base de datos local
(con posibilidad futura de migrar a la nube).

------------------------------------------------------------
🚀 Características principales
------------------------------------------------------------
- CRUD completo de entidades principales (Usuarios, Organizadores, Organizaciones, Economía).
- Generación automática de códigos únicos de economía (CodigoEconomia).
- Registro y autenticación de usuarios mediante Email y PasswordHash.
- Control de ingresos y egresos asociados a una organización.
- Roles de organizadores (Owner / Admin / View).
- Cálculo total de movimientos económicos.
- Eliminación en cascada de registros relacionados.
- Arquitectura modular con separación por capas (Controllers, Entities, Dtos, Database).

------------------------------------------------------------
🧱 Estructura del proyecto
------------------------------------------------------------
📁 Proyecto1.API
 ┣ 📂 Controllers
 ┃ ┣ EndpointsEconomia.cs
 ┃ ┣ EndpointsOrganizadores.cs
 ┃ ┣ EndpointsOrgEconomica.cs
 ┃ ┗ EndpointsUser.cs
 ┣ 📂 DataBase
 ┃ ┣ 📂 Entities
 ┃ ┃ ┣ Economia.cs
 ┃ ┃ ┣ OrganizacionEconomica.cs
 ┃ ┃ ┣ Organizador.cs
 ┃ ┃ ┗ Usuario.cs
 ┃ ┗ MyAppDbContext.cs
 ┣ 📂 Dtos
 ┃ ┣ DtoAccederOrgEconm.cs
 ┃ ┣ DtoHistorialEconomia.cs
 ┃ ┣ DtoOrganizador.cs
 ┃ ┣ DtoOrgEconm.cs
 ┃ ┗ DtoUser.cs
 ┗ Program.cs / Startup.cs

------------------------------------------------------------
🧩 Entidades principales
------------------------------------------------------------
Usuarios
- Id
- Email
- PasswordHash
- CodigoUsuario

Organizadores
- Id
- FKId_Usuarios
- tipoUsuario (Owner/Admin/Viewer)
- FKCodigoEconomia

OrganizacionEconomica
- Id
- NombreOrg
- PasswordOrgHash
- CodigoEconomia
- FKCodigoUsuario

Economia
- Id
- NumMovimiento
- DescripcionMov
- IngresoEgreso
- FechaIngrEgr
- FKCodigoEconomia

------------------------------------------------------------
🔗 Endpoints disponibles
------------------------------------------------------------
POST /api/user/create                -> Crear usuario
POST /api/organizacion/create        -> Crear organización
POST /api/user/login                 -> Iniciar sesión
GET /api/economia/historial/{codigo} -> Ver historial de movimientos
GET /api/economia/total/{codigo}     -> Calcular total de movimientos
POST /api/economia/agregar           -> Agregar ingreso o egreso
DELETE /api/economia/eliminar/{id}   -> Eliminar registro
PUT /api/organizador/update/{id}     -> Actualizar organizador
DELETE /api/economia/reiniciar/{codigo} -> Reiniciar economía
POST /api/organizador/agregar        -> Añadir administrador o visitante
DELETE /api/organizador/eliminar/{id} -> Eliminar organizador
DELETE /api/organizacion/eliminar/{codigo} -> Eliminar organización
DELETE /api/user/eliminar/{email}    -> Eliminar cuenta

------------------------------------------------------------
💾 Ejemplo de JSON (Crear usuario)
------------------------------------------------------------
POST /api/user/create
{
  "email": "ejemplo@correo.com",
  "password": "MiPassword123"
}

Respuesta:
{
  "codigoUsuario": "US_98563X",
  "mensaje": "Usuario creado exitosamente"
}

------------------------------------------------------------
🧠 Lógica del flujo general
------------------------------------------------------------
1. El usuario se registra → se genera CodigoUsuario.
2. Crea una organización → se genera CodigoEconomia.
3. Se asocia como Owner y puede agregar administradores o visitantes.
4. Agrega movimientos (ingresos/egresos).
5. Puede consultar historial y totales.
6. En caso de eliminación de cuenta, todos los registros asociados se eliminan en cascada.

------------------------------------------------------------
🧰 Tecnologías utilizadas
------------------------------------------------------------
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- C# 12
- Swagger / Swashbuckle
- SQL Server LocalDB (actualmente)
- PostgreSQL o Azure SQL (plan futuro)

------------------------------------------------------------
⚙️ Instrucciones de instalación
------------------------------------------------------------
1. Clonar el repositorio:
   git clone https://github.com/tuusuario/Proyecto1-ApiGestionEconomia.git

2. Acceder al directorio:
   cd Proyecto1-ApiGestionEconomia

3. Restaurar paquetes:
   dotnet restore

4. Ejecutar migraciones (si se usa EF Core):
   dotnet ef database update

5. Iniciar la API:
   dotnet run

6. Acceder a Swagger:
   https://localhost:7148/swagger

------------------------------------------------------------
🧭 Próximas mejoras
------------------------------------------------------------
- Implementación de autenticación JWT.
- Middleware global para manejo de errores.
- Migración de base de datos a la nube (Azure SQL o PostgreSQL).
- Integración de logs con Serilog.
- Pruebas unitarias (xUnit / MSTest).
- Validaciones de modelo con DataAnnotations.

------------------------------------------------------------
🧑‍💻 Autor
------------------------------------------------------------
Desarrollador: GieziAdael
Rol: Backend Developer (.NET Junior)
Contacto: giezi.tlaxcoapan@gmail.com

------------------------------------------------------------
🏁 Estado del proyecto
------------------------------------------------------------
En desarrollo activo
Actualmente la API se ejecuta en entorno local, con planes de expansión a la nube
y mejoras en seguridad y documentación.
