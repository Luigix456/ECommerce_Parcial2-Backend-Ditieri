# ECommerce API — Clean Architecture .NET 8

Proyecto completo de e-commerce para Backend 2026.

Incluye:

- Clean Architecture: `Domain`, `Application`, `Infrastructure`, `Api`.
- Entidades de dominio: `Product`, `Category`, `Order`, `OrderItem`, `User`.
- EF Core + SQLite.
- Fluent API por entidad.
- Repositorios en Infrastructure e interfaces en Application.
- DTOs y Mappers para no exponer entidades directamente.
- JWT con login, claims y roles.
- Endpoints protegidos con `[Authorize]` y `[Authorize(Roles = "Admin")]`.
- Swagger con botón `Authorize` para probar JWT.
- GlobalExceptionHandler + ProblemDetails.
- FluentValidation para requests.
- Seed automático de categorías, productos y usuarios demo.

## Usuarios de prueba

Admin:

```json
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
```

Usuario común:

```json
{
  "email": "user@test.com",
  "password": "User123!"
}
```

## Comandos iniciales

Desde la carpeta raíz donde está `ECommerce.sln`:

```bash
dotnet restore
```

Instalar la herramienta de EF Core si no la tenés:

```bash
dotnet tool install --global dotnet-ef
```

Crear migración inicial:

```bash
dotnet ef migrations add InitialCreate --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

Aplicar migración:

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

Ejecutar API:

```bash
dotnet run --project ECommerce.Api
```

Swagger:

```text
https://localhost:7074/swagger
```

> Nota: el proyecto también ejecuta `DataSeeder.SeedAsync()` al arrancar. Si ya hay migraciones aplicadas, crea datos iniciales automáticamente.

## Flujo de prueba recomendado

1. Entrar a Swagger.
2. Ejecutar `POST /api/auth/login` con `admin@test.com` y `Admin123!`.
3. Copiar el token.
4. Presionar `Authorize` en Swagger y pegar el token.
5. Probar `POST /api/products` para crear un producto.
6. Probar `GET /api/products/paged?page=1&pageSize=2`.
7. Hacer login con `user@test.com`.
8. Crear una orden en `POST /api/orders` usando el token del usuario.
9. Ver `GET /api/orders/my-orders`.
10. Intentar borrar un producto con token de usuario común: debe devolver 403.

## Endpoints principales

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`

### Categories

- `GET /api/categories`
- `GET /api/categories/{id}`

### Products

- `GET /api/products`
- `GET /api/products/paged?page=1&pageSize=10`
- `GET /api/products/search?term=mouse`
- `GET /api/products/{id}`
- `POST /api/products` — Admin
- `PUT /api/products/{id}` — Admin
- `DELETE /api/products/{id}` — Admin

### Orders

- `POST /api/orders` — usuario autenticado
- `GET /api/orders/my-orders` — usuario autenticado
- `GET /api/orders/{id}` — dueño de la orden o Admin

## Categorías seed

```text
Electrónica: a1b2c3d4-0000-0000-0000-000000000001
Ropa:        a1b2c3d4-0000-0000-0000-000000000002
Hogar:       a1b2c3d4-0000-0000-0000-000000000003
```

## Validaciones implementadas

`CreateProductRequest` y `UpdateProductRequest` validan:

- `Name`: obligatorio y máximo 100 caracteres.
- `Description`: máximo 500 caracteres.
- `Price`: mayor a cero.
- `Stock`: mayor o igual a cero.
- `CategoryId`: obligatorio.

`CreateOrderRequest` valida:

- La orden debe tener al menos un item.
- Cada item debe tener `ProductId`.
- Cada item debe tener `Quantity > 0`.

## Errores esperados

Producto inexistente:

```http
GET /api/products/00000000-0000-0000-0000-000000000000
```

Respuesta esperada: `404 Not Found` con ProblemDetails.

Stock insuficiente:

```http
POST /api/orders
```

con una cantidad mayor al stock disponible.

Respuesta esperada: `422 Unprocessable Entity` con ProblemDetails.

Sin token:

```http
POST /api/orders
```

Respuesta esperada: `401 Unauthorized`.

Token válido pero sin rol Admin:

```http
DELETE /api/products/{id}
```

Respuesta esperada: `403 Forbidden`.
