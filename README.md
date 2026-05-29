# ECommerce Backend API

API backend para un sistema de e-commerce desarrollada en **.NET 8**, aplicando **Clean Architecture**, **CQRS con MediatR**, **Entity Framework Core**, **JWT**, **DTOs**, **validaciones**, **repositorios** y **migraciones**.

El proyecto respeta el flujo solicitado:

```text
Controller → IMediator → Handler → Repository → DbContext
```

---

## 1. Objetivo del proyecto

El objetivo del proyecto es implementar una API backend para un e-commerce, permitiendo:

- Autenticación de usuarios mediante JWT.
- Consulta de categorías.
- Consulta paginada de productos.
- Creación de productos por usuarios administradores.
- Creación de órdenes de compra por usuarios autenticados.
- Consulta de órdenes propias.
- Persistencia de datos con Entity Framework Core.
- Aplicación de Clean Architecture y CQRS.

---

## 2. Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- MediatR
- CQRS
- JWT Bearer Authentication
- FluentValidation
- Swagger / OpenAPI
- BCrypt para hash de contraseñas

---

## 3. Estructura general del proyecto

```text
ECommerce_Backend
├── ECommerce.Api
├── ECommerce.Application
├── ECommerce.Domain
└── ECommerce.Infrastructure
```

---

## 4. Capas del proyecto

## 4.1 ECommerce.Api

La capa `Api` es la capa de presentación HTTP.

Contiene:

```text
Controllers
Program.cs
appsettings.json
Swagger
Configuración JWT
```

Responsabilidades:

- Recibir solicitudes HTTP.
- Exponer endpoints REST.
- Usar `IMediator.Send()` para enviar Commands o Queries.
- Devolver respuestas HTTP.
- Aplicar autorización mediante `[Authorize]`.
- Configurar Swagger.
- Configurar autenticación JWT.

Los controllers no acceden directamente a repositorios ni al `DbContext`.

Ejemplo:

```csharp
var result = await _mediator.Send(command, cancellationToken);
return Ok(result);
```

---

## 4.2 ECommerce.Application

La capa `Application` contiene la lógica de aplicación y la implementación de CQRS.

Contiene:

```text
Commands
Queries
Handlers
DTOs
Interfaces
Mappings
Validators
Configuration
Common
```

Responsabilidades:

- Definir Commands.
- Definir Queries.
- Implementar Handlers de MediatR.
- Definir DTOs de entrada y salida.
- Definir interfaces de repositorios y servicios.
- Ejecutar validaciones con FluentValidation.
- Coordinar reglas de aplicación.

Esta capa no conoce detalles técnicos de base de datos, JWT concreto, BCrypt ni Entity Framework.

---

## 4.3 ECommerce.Domain

La capa `Domain` contiene el modelo puro del negocio.

Contiene:

```text
Entities
Exceptions
ValueObjects
```

Responsabilidades:

- Definir entidades del negocio.
- Encapsular reglas de dominio.
- Definir excepciones propias del dominio.
- Mantenerse independiente de frameworks externos.

Ejemplos de entidades:

```text
Product
Category
User
Order
OrderItem
```

El dominio no conoce:

```text
HTTP
Controllers
Swagger
Entity Framework
JWT
MediatR
SQLite
```

---

## 4.4 ECommerce.Infrastructure

La capa `Infrastructure` implementa detalles técnicos.

Contiene:

```text
Persistence
Repositories
Configuration
Migrations
Services
```

Responsabilidades:

- Implementar repositorios usando EF Core.
- Configurar `ApplicationDbContext`.
- Definir configuraciones Fluent API.
- Gestionar migraciones.
- Implementar servicios concretos como JWT y BCrypt.
- Ejecutar seed inicial de datos.

Ejemplos:

```text
ProductRepository
CategoryRepository
OrderRepository
UserRepository
JwtTokenService
BCryptPasswordHasher
ApplicationDbContext
```

---

# 5. Flujo solicitado

El flujo principal del proyecto es:

```text
HTTP Request
   ↓
Controller
   ↓
IMediator.Send(command/query)
   ↓
Handler
   ↓
Repository Interface
   ↓
Repository Implementation
   ↓
ApplicationDbContext
   ↓
Database
```

Este flujo evita que los controllers accedan directamente a repositorios o a Entity Framework.

---

## 5.1 Ejemplo: crear producto

```text
POST /api/Products
   ↓
ProductsController
   ↓
CreateProductCommand
   ↓
CreateProductCommandHandler
   ↓
IProductRepository / ICategoryRepository
   ↓
ProductRepository / CategoryRepository
   ↓
ApplicationDbContext
   ↓
SQLite
```

---

## 5.2 Ejemplo: consultar productos paginados

```text
GET /api/Products/paged?page=1&pageSize=10
   ↓
ProductsController
   ↓
GetPagedProductsQuery
   ↓
GetPagedProductsQueryHandler
   ↓
IProductRepository
   ↓
ProductRepository
   ↓
ApplicationDbContext
   ↓
SQLite
```

---

## 5.3 Ejemplo: consultar categorías

```text
GET /api/Categories
   ↓
CategoriesController
   ↓
GetAllCategoriesQuery
   ↓
GetAllCategoriesQueryHandler
   ↓
ICategoryRepository
   ↓
CategoryRepository
   ↓
ApplicationDbContext
   ↓
SQLite
```

---

# 6. CQRS

El proyecto aplica CQRS separando las operaciones de escritura y lectura.

---

## 6.1 Commands

Los Commands representan acciones que modifican el estado del sistema.

Ejemplos:

```text
CreateProductCommand
CreateOrderCommand
LoginCommand
```

Cada Command tiene su Handler correspondiente.

Ejemplo:

```text
CreateProductCommand
   ↓
CreateProductCommandHandler
```

---

## 6.2 Queries

Las Queries representan operaciones de lectura. No modifican el estado del sistema.

Ejemplos:

```text
GetPagedProductsQuery
GetAllCategoriesQuery
GetMyOrdersQuery
```

Cada Query tiene su Handler correspondiente.

Ejemplo:

```text
GetPagedProductsQuery
   ↓
GetPagedProductsQueryHandler
```

---

# 7. Casos obligatorios implementados

## 7.1 Command obligatorio

```text
CreateProductCommand
CreateProductCommandHandler
```

Este Command permite crear un producto y modifica el estado de la base de datos.

---

## 7.2 Query obligatoria

```text
GetPagedProductsQuery
GetPagedProductsQueryHandler
```

Esta Query permite consultar productos paginados sin modificar datos.

---

## 7.3 Endpoint GET /categories

Implementado mediante CQRS:

```text
GET /api/Categories
   ↓
CategoriesController
   ↓
GetAllCategoriesQuery
   ↓
GetAllCategoriesQueryHandler
```

---

## 7.4 JWT

El proyecto utiliza autenticación mediante JWT.

Flujo:

```text
POST /api/Auth/login
   ↓
LoginCommand
   ↓
LoginCommandHandler
   ↓
IUserRepository
   ↓
IPasswordHasher
   ↓
ITokenService
   ↓
JWT Token
```

Los endpoints protegidos usan:

```csharp
[Authorize]
```

o:

```csharp
[Authorize(Roles = "Admin")]
```

---

# 8. Entidades principales

## 8.1 Product

Representa un producto del e-commerce.

Propiedades principales:

```text
Id
Name
Description
Price
Stock
CategoryId
CreatedAt
```

---

## 8.2 Category

Representa una categoría de productos.

Propiedades principales:

```text
Id
Name
```

---

## 8.3 User

Representa un usuario del sistema.

Propiedades principales:

```text
Id
Email
Name
PasswordHash
Role
CreatedAt
```

Roles utilizados:

```text
Admin
User
```

---

## 8.4 Order

Representa una orden de compra.

Propiedades principales:

```text
Id
UserId
CreatedAt
Status
Total
Items
```

---

## 8.5 OrderItem

Representa un ítem dentro de una orden.

Propiedades principales:

```text
Id
OrderId
ProductId
UnitPrice
Quantity
Subtotal
```

---

# 9. Repositorios

Las interfaces se definen en `Application`:

```text
IProductRepository
ICategoryRepository
IOrderRepository
IUserRepository
IPasswordHasher
ITokenService
```

Las implementaciones se encuentran en `Infrastructure`:

```text
ProductRepository
CategoryRepository
OrderRepository
UserRepository
BCryptPasswordHasher
JwtTokenService
```

Esto respeta la inversión de dependencias:

```text
Application define qué necesita.
Infrastructure define cómo se implementa.
```

---

# 10. DTOs

Los DTOs se encuentran en la capa `Application`.

Ejemplos:

```text
ProductDto
CategoryDto
OrderDto
OrderItemDto
AuthResponseDto
PagedResponse<T>
```

Los DTOs evitan exponer directamente las entidades de dominio y permiten controlar qué datos devuelve la API.

---

# 11. Validaciones

Las validaciones están en la capa `Application`.

Ejemplos:

```text
CreateProductCommandValidator
CreateOrderCommandValidator
LoginCommandValidator
```

Se utiliza FluentValidation para validar Commands antes de ejecutar los Handlers.

Ejemplo de validaciones:

- El nombre del producto es obligatorio.
- El precio debe ser mayor a cero.
- El stock no puede ser negativo.
- La orden debe tener al menos un producto.
- La cantidad de cada ítem debe ser mayor a cero.
- El email de login es obligatorio y debe tener formato válido.

---

# 12. Base de datos

El proyecto utiliza SQLite para desarrollo local.

La connection string está en:

```text
ECommerce.Api/appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ecommerce.db"
  }
}
```

---

# 13. Entity Framework Core

La persistencia se implementa con Entity Framework Core.

Archivos principales:

```text
ApplicationDbContext
ProductConfiguration
CategoryConfiguration
UserConfiguration
OrderConfiguration
OrderItemConfiguration
```

El `ApplicationDbContext` se encuentra en:

```text
ECommerce.Infrastructure/Persistence/ApplicationDbContext.cs
```

Las configuraciones Fluent API se aplican automáticamente mediante:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
```

---

# 14. Migraciones

Las migraciones se generan en el proyecto `ECommerce.Infrastructure`.

## 14.1 Crear migración inicial

```bash
dotnet ef migrations add InitialCreate --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

Si ya existe una migración llamada `InitialCreate`, usar otro nombre:

```bash
dotnet ef migrations add InitialCreateV2 --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

---

## 14.2 Aplicar migraciones

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

---

## 14.3 Ejecutar el proyecto

```bash
dotnet run --project ECommerce.Api
```

---

# 15. Seed inicial

El proyecto carga usuarios iniciales para pruebas.

## Usuario administrador

```json
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
```

Rol:

```text
Admin
```

---

## Usuario normal

```json
{
  "email": "user@test.com",
  "password": "User123!"
}
```

Rol:

```text
User
```

También se cargan categorías iniciales para poder crear productos.

---

# 16. Swagger

Una vez ejecutada la API, abrir Swagger en la URL indicada por consola.

Ejemplo:

```text
https://localhost:7074/swagger
```

Desde Swagger se puede:

- Probar login.
- Copiar el token JWT.
- Autorizar usando `Bearer {token}`.
- Consultar categorías.
- Consultar productos paginados.
- Crear productos como administrador.
- Crear órdenes como usuario autenticado.
- Consultar órdenes propias.

---

# 17. Instructivo de ejecución desde cero

## Paso 1: Restaurar paquetes

```bash
dotnet restore
```

---

## Paso 2: Compilar

```bash
dotnet build
```

---

## Paso 3: Crear la migración inicial

```bash
dotnet ef migrations add InitialCreate --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

Si la migración ya existe, no hace falta volver a crearla.

---

## Paso 4: Aplicar migraciones

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

---

## Paso 5: Ejecutar la API

```bash
dotnet run --project ECommerce.Api
```

---

## Paso 6: Abrir Swagger

Ir a:

```text
https://localhost:7074/swagger
```

o al puerto que indique la consola.

---

# 18. Instructivo de pruebas

## 18.1 Consultar categorías

Endpoint:

```http
GET /api/Categories
```

Resultado esperado:

```json
[
  {
    "id": "guid",
    "name": "Electrónica"
  },
  {
    "id": "guid",
    "name": "Ropa"
  },
  {
    "id": "guid",
    "name": "Hogar"
  }
]
```

Copiar un `id` de categoría para crear productos.

---

## 18.2 Login como administrador

Endpoint:

```http
POST /api/Auth/login
```

Body:

```json
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
```

Respuesta esperada:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---

## 18.3 Autorizar Swagger

Presionar el botón:

```text
Authorize
```

Ingresar:

```text
Bearer TOKEN_GENERADO
```

Ejemplo:

```text
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
```

---

## 18.4 Crear producto

Endpoint:

```http
POST /api/Products
```

Requiere token de administrador.

Body:

```json
{
  "name": "Notebook Lenovo ThinkPad",
  "description": "Notebook para desarrollo backend",
  "price": 1200000,
  "stock": 10,
  "categoryId": "PEGAR_ID_DE_CATEGORIA"
}
```

Respuesta esperada:

```json
{
  "id": "guid",
  "name": "Notebook Lenovo ThinkPad",
  "description": "Notebook para desarrollo backend",
  "price": 1200000,
  "stock": 10,
  "categoryId": "guid",
  "categoryName": "Electrónica"
}
```

---

## 18.5 Consultar productos paginados

Endpoint:

```http
GET /api/Products/paged?page=1&pageSize=10
```

Respuesta esperada:

```json
{
  "items": [
    {
      "id": "guid",
      "name": "Notebook Lenovo ThinkPad",
      "description": "Notebook para desarrollo backend",
      "price": 1200000,
      "stock": 10,
      "categoryId": "guid",
      "categoryName": "Electrónica"
    }
  ],
  "totalCount": 1,
  "totalPages": 1,
  "currentPage": 1,
  "pageSize": 10
}
```

---

## 18.6 Login como usuario normal

Endpoint:

```http
POST /api/Auth/login
```

Body:

```json
{
  "email": "user@test.com",
  "password": "User123!"
}
```

Copiar el token y autorizar Swagger nuevamente.

---

## 18.7 Crear orden

Endpoint:

```http
POST /api/Orders
```

Requiere usuario autenticado.

Body:

```json
{
  "items": [
    {
      "productId": "PEGAR_ID_DEL_PRODUCTO",
      "quantity": 2
    }
  ]
}
```

Respuesta esperada:

```json
{
  "id": "guid",
  "userId": "guid",
  "status": "Pending",
  "total": 2400000,
  "createdAt": "fecha",
  "items": [
    {
      "productId": "guid",
      "unitPrice": 1200000,
      "quantity": 2,
      "subtotal": 2400000
    }
  ]
}
```

---

## 18.8 Consultar mis órdenes

Endpoint:

```http
GET /api/Orders/my-orders
```

Requiere usuario autenticado.

Respuesta esperada:

```json
[
  {
    "id": "guid",
    "userId": "guid",
    "status": "Pending",
    "total": 2400000,
    "createdAt": "fecha",
    "items": [
      {
        "productId": "guid",
        "unitPrice": 1200000,
        "quantity": 2,
        "subtotal": 2400000
      }
    ]
  }
]
```

---

# 19. Pruebas de seguridad

## 19.1 Endpoint protegido sin token

Intentar crear un producto sin token:

```http
POST /api/Products
```

Resultado esperado:

```text
401 Unauthorized
```

---

## 19.2 Usuario normal intentando crear producto

Loguearse con:

```json
{
  "email": "user@test.com",
  "password": "User123!"
}
```

Intentar:

```http
POST /api/Products
```

Resultado esperado:

```text
403 Forbidden
```

Esto ocurre porque crear productos requiere rol:

```csharp
[Authorize(Roles = "Admin")]
```

---

# 20. Pruebas de validación

## 20.1 Crear producto inválido

Endpoint:

```http
POST /api/Products
```

Body inválido:

```json
{
  "name": "",
  "description": "Producto inválido",
  "price": -100,
  "stock": -5,
  "categoryId": "00000000-0000-0000-0000-000000000000"
}
```

Resultado esperado:

```text
400 Bad Request
```

---

## 20.2 Crear orden con cantidad inválida

Endpoint:

```http
POST /api/Orders
```

Body inválido:

```json
{
  "items": [
    {
      "productId": "PEGAR_ID_DEL_PRODUCTO",
      "quantity": 0
    }
  ]
}
```

Resultado esperado:

```text
400 Bad Request
```

---

# 21. Prueba de regla de negocio

Intentar crear una orden con una cantidad mayor al stock disponible.

Endpoint:

```http
POST /api/Orders
```

Body:

```json
{
  "items": [
    {
      "productId": "PEGAR_ID_DEL_PRODUCTO",
      "quantity": 99999
    }
  ]
}
```

Resultado esperado:

```text
422 Unprocessable Entity
```

o un mensaje de error indicando stock insuficiente.

---

# 22. Comandos útiles

## Restaurar paquetes

```bash
dotnet restore
```

## Compilar

```bash
dotnet build
```

## Ejecutar

```bash
dotnet run --project ECommerce.Api
```

## Crear migración

```bash
dotnet ef migrations add NombreMigracion --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

## Aplicar migración

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

## Listar migraciones

```bash
dotnet ef migrations list --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

## Eliminar última migración no aplicada

```bash
dotnet ef migrations remove --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

## Eliminar base de datos

```bash
dotnet ef database drop --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

---

# 23. Resumen técnico final

Este proyecto implementa una API backend de e-commerce aplicando Clean Architecture y CQRS con MediatR.

La API no accede directamente a repositorios ni a Entity Framework. En su lugar, cada solicitud HTTP es recibida por un Controller, que envía un Command o Query mediante `IMediator`.

Luego, MediatR resuelve el Handler correspondiente. El Handler usa interfaces de repositorio definidas en Application, y las implementaciones concretas se encuentran en Infrastructure usando EF Core y `ApplicationDbContext`.

Flujo final:

```text
Controller → IMediator → Handler → Repository → DbContext
```

Este diseño permite separar responsabilidades, mejorar la mantenibilidad, facilitar pruebas y respetar la arquitectura solicitada.
