# NayeliApi - API Bancaria

API RESTful para la gestión de cuentas bancarias y transacciones, desarrollada en .NET 8 como parte de una prueba técnica de programador.

## Descripción

Esta API permite a los usuarios gestionar perfiles de clientes, crear cuentas bancarias, realizar transacciones (depósitos y retiros), consultar saldos y obtener el historial de transacciones.

## Características

- Creación de perfil de cliente con información personal
- Creación de cuentas bancarias asociadas a clientes
- Realización de depósitos y retiros con validación de saldo
- Consulta de saldo de cuentas
- Historial completo de transacciones
- Resumen de transacciones con totales y saldo final
- Documentación interactiva con Swagger/OpenAPI
- Pruebas unitarias con xUnit

## Tecnologías Utilizadas

- .NET 8 
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger
- xUnit (Para los Test Unitarios Estándar Moderno)
- Moq

## Arquitectura

El proyecto sigue una arquitectura en capas (Arquitectura Limpia):

```
NayeliApi/
├── NayeliApi.Api/              # Capa de presentación (Controllers, Program.cs)
├── NayeliApi.Core/             # Capa de dominio (Entidades, Interfaces, Servicios)
├── NayeliApi.Infrastructure/   # Capa de infraestructura (DbContext, Repositories)
└── NayeliApi.Tests/            # Pruebas unitarias
```

### Principios SOLID Aplicados

1. **Single Responsibility Principle**: Cada clase tiene una única responsabilidad
2. **Open/Closed Principle**: Extensibilidad mediante interfaces
3. **Liskov Substitution Principle**: Uso de abstracciones intercambiables
4. **Interface Segregation Principle**: Interfaces específicas por funcionalidad
5. **Dependency Inversion Principle**: Inyección de dependencias

## Requisitos Previos

- .NET 8 SDK ([Descargar aquí](https://dotnet.microsoft.com/download/dotnet/8.0))
- Editor de código (Visual Studio 2022, Visual Studio Code, o Rider)

## Instalación y Ejecución

### 1. Clonar el repositorio

```bash
git clone <https://github.com/naye-19/Prueba_Tecnica_ProgramadorJr.git>
cd Prueba_Tecnica_ProgramadorJr/NayeliApi
```

### 2. Restaurar paquetes NuGet

```bash
dotnet restore
```

### 3. Crear y aplicar migraciones de base de datos

```bash
dotnet ef migrations add InitialCreate --project NayeliApi.Infrastructure --startup-project NayeliApi.Api
dotnet ef database update --project NayeliApi.Infrastructure --startup-project NayeliApi.Api
```

### 4. Ejecutar la API

```bash
dotnet run --project NayeliApi.Api
```

La API estará disponible en:
- HTTPS: `https://localhost:7164`
- HTTP: `http://localhost:5139`

### 5. Acceder a Swagger

Abrir en el navegador:
```
https://localhost:7164/swagger
```

## Endpoints de la API

### Clientes

#### Crear Cliente
```http
POST /api/clientes
Content-Type: application/json

{
  "nombre": "Juan Pérez",
  "fechaNacimiento": "1990-05-15",
  "sexo": "Masculino",
  "ingresos": 50000
}
```

#### Obtener Cliente
```http
GET /api/clientes/{id}
```

### Cuentas Bancarias

#### Crear Cuenta
```http
POST /api/cuentas
Content-Type: application/json

{
  "clienteId": "guid-del-cliente"
}
```

#### Consultar Saldo
```http
GET /api/cuentas/{numeroCuenta}/saldo
```

### Transacciones

#### Realizar Depósito
```http
POST /api/transacciones/deposito
Content-Type: application/json

{
  "numeroCuenta": "ACC123456789ABC",
  "monto": 500.00
}
```

#### Realizar Retiro
```http
POST /api/transacciones/retiro
Content-Type: application/json

{
  "numeroCuenta": "ACC123456789ABC",
  "monto": 300.00
}
```

#### Obtener Historial
```http
GET /api/transacciones/{numeroCuenta}/historial
```

#### Obtener Resumen
```http
GET /api/transacciones/{numeroCuenta}/resumen
```

## Pruebas Unitarias

### Ejecutar todas las pruebas

```bash
dotnet test
```

### Ejecutar pruebas con detalles

```bash
dotnet test --verbosity detailed
```

### Cobertura de pruebas

Las pruebas unitarias cubren:
- Creación de clientes con validaciones
- Creación de cuentas bancarias
- Depósitos y retiros
- Validación de saldo insuficiente
- Consultas de saldo e historial
- Cálculo de resúmenes de transacciones

## Ejemplo de Flujo Completo

1. **Crear un cliente**
```json
POST /api/clientes
{
  "nombre": "María López",
  "fechaNacimiento": "1985-08-20",
  "sexo": "Femenino",
  "ingresos": 75000
}
```

2. **Crear una cuenta para el cliente**
```json
POST /api/cuentas
{
  "clienteId": "id-obtenido-del-paso-1"
}
```

3. **Realizar un depósito**
```json
POST /api/transacciones/deposito
{
  "numeroCuenta": "cuenta-obtenida-del-paso-2",
  "monto": 1000.00
}
```

4. **Consultar saldo**
```http
GET /api/cuentas/{numeroCuenta]/saldo
```

5. **Realizar un retiro**
```json
POST /api/transacciones/retiro
{
  "numeroCuenta": "ACC123456789ABC",
  "monto": 300.00
}
```

6. **Obtener resumen de transacciones**
```http
GET /api/transacciones/{numeroCuenta}/resumen
```

## Reglas de Negocio

1. Un cliente debe tener todos los campos obligatorios (nombre, fecha de nacimiento, sexo, ingresos)
2. Una cuenta bancaria se crea con saldo inicial de 0
3. El número de cuenta se genera automáticamente con formato "BCCEHNxxxxxxxxx"
4. Los depósitos deben tener un monto mayor a 0
5. Los retiros solo se permiten si hay saldo suficiente
6. Cada transacción registra el saldo después de la operación
7. El historial de transacciones se ordena por fecha Descendente
8. El resumen se muestra en el orden en que fueron realizadas las transacciones para seguir el flujo. (Ascendente)

## Manejo de Errores

La API retorna códigos HTTP estándar:

- `200 OK`: Operación exitosa
- `201 Created`: Recurso creado exitosamente
- `400 Bad Request`: Datos inválidos o saldo insuficiente
- `404 Not Found`: Recurso no encontrado
- `500 Internal Server Error`: Error del servidor

Formato de error:
```json
{
  "mensaje": "Descripción del error"
}
```

## Base de Datos

La aplicación usa SQLite con Entity Framework Core. El archivo de base de datos (`banco.db`) se crea automáticamente en la carpeta del proyecto API al ejecutar las migraciones.

## Desarrollo

### Estructura de Carpetas

```
NayeliApi.Core/
├── Entities/           # Entidades del dominio
├── DTOs/               # Data Transfer Objects
├── Interfaces/         # Contratos de servicios y repositorios
├── Services/           # Lógica de negocio
└── Exceptions/         # Excepciones personalizadas

NayeliApi.Infrastructure/
├── Data/               # DbContext
├── Migrations/         # Migraciones de base de datos.
└── Repositories/       # Implementación de repositorios

NayeliApi.Api/
└── Controllers/        # Endpoints REST

NayeliApi.Tests/
└── Services/           # Pruebas unitarias de servicios
```

## Autor

Desarrollado como parte de una prueba técnica para evaluar conocimientos de programación en .NET. por Nayeli Benavides

## Duración Estimada

4 horas de desarrollo

## Licencia

Este proyecto fue creado con fines educativos y de evaluación técnica.
