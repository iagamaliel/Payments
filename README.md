# ESolutions Payments

Aplicación web para la consulta y selección de compañías de servicios, desarrollada con **C#, ASP.NET Web Forms, ADO.NET y SQL Server**.

La solución permite consultar las compañías disponibles según el canal solicitado, validar el acceso mediante API Key y navegar a una página de destino después de seleccionar una compañía.

La configuración de los canales se administra desde SQL Server, permitiendo incorporar nuevos canales sin modificar el código de la aplicación.

---

## 1. Descripción general

### Funcionalidades principales

- Configuración dinámica de canales almacenada en SQL Server.
- Servicio REST para consultar el catálogo de compañías.
- Autenticación mediante API Key utilizando el encabezado HTTP `X-Api-Key`.
- Configuración dinámica de `ServiceUrl` y `DestinationPage`.
- Visualización dinámica del menú según la configuración del canal.
- Selección de compañías y navegación mediante Session.
- Manejo controlado de errores de autenticación, configuración y comunicación con el servicio.
- Consumo asíncrono del servicio desde ASP.NET Web Forms.

---

## 2. Arquitectura de la solución

La solución está organizada en tres proyectos principales, aplicando separación de responsabilidades:

```text
ESolutions.Payments
│
├── ESolutions.Payments.Business
│
├── ESolutions.Payments.Data
│
└── ESolutions.Payments.Web
```

### 2.1 Business Layer

**Proyecto:** `ESolutions.Payments.Business`

Contiene los modelos, contratos y lógica de negocio de la aplicación.

Responsabilidades:

- Definir los modelos de compañías y configuración de canales.
- Definir interfaces de servicios y repositorios.
- Implementar las reglas de negocio.
- Consumir el servicio de compañías.
- Gestionar las excepciones relacionadas con los servicios.

Componentes representativos:

```text
Business
│
├── Models
│   ├── Company.cs
│   └── ChannelConfiguration.cs
│
├── Interfaces
│   ├── ICompanyService.cs
│   └── IChannelRepository.cs
│
└── Services
    └── CompanyService.cs
```

### 2.2 Data Layer

**Proyecto:** `ESolutions.Payments.Data`

Responsable del acceso a datos mediante ADO.NET.

Responsabilidades:

- Administrar conexiones a SQL Server.
- Consultar configuraciones de canales.
- Ejecutar procedimientos almacenados.
- Consultar información de compañías desde su fuente de datos.
- Convertir los resultados de SQL Server en modelos de la aplicación.

Componentes representativos:

```text
Data
│
├── Repositories
│   ├── ChannelRepository.cs
│   └── CompanyRepository.cs
│
└── Database
    └── SQL Server access
```

La configuración de canales se recupera mediante el procedimiento almacenado:

```sql
dbo.usp_GetChannelConfiguration
```

Este procedimiento recibe el código del canal y devuelve su configuración activa.

### 2.3 Web Layer

**Proyecto:** `ESolutions.Payments.Web`

Responsable de la interfaz de usuario, los endpoints HTTP, la comunicación entre capas y la navegación de la aplicación.

Responsabilidades:

- Mostrar la página de selección de compañías.
- Exponer los endpoints REST.
- Validar la autenticación mediante API Key.
- Consumir los servicios de negocio.
- Administrar la información de Session.
- Gestionar la navegación hacia la página de destino.

Componentes representativos:

```text
Web
│
├── App_Start
│   └── WebApiConfig.cs
│
├── Controllers
│   └── CompaniesController.cs
│
├── Companies.aspx
├── Companies.aspx.cs
│
├── Destination.aspx
├── Destination.aspx.cs
│
├── Content
│   └── Images
│       └── company-placeholder.png
│
├── Global.asax
└── Web.config
```

## 3. Configuración de la base de datos

La aplicación utiliza una base de datos denominada:

```text
ESolutionsPayments
```

Los scripts SQL se encuentran organizados dentro de la carpeta `Database`.

```text
Database
│
├── 01_CreateDatabase.sql
├── 02_CreateTables.sql
├── 03_InsertInitialData.sql
└── 04_CreateStoredProcedures.sql
```

### 3.1 Entornos compatibles

La base de datos puede crearse utilizando cualquiera de estas opciones:

- SQL Server Express.
- SQL Server LocalDB.

Es necesario tener instalado SQL Server y SQL Server Management Studio (SSMS), si corresponde.

### 3.2 Creación de la base de datos

Abrir SQL Server Management Studio y conectarse a la instancia de SQL Server.

Para SQL Server Express, una conexión habitual es:

```text
.\SQLEXPRESS
```

Para SQL Server LocalDB:

```text
(localdb)\MSSQLLocalDB
```

Ejecutar los scripts en el siguiente orden:

| Orden | Script | Descripción |
|---|---|---|
| 1 | `01_CreateDatabase.sql` | Crea la base de datos. |
| 2 | `02_CreateTables.sql` | Crea las tablas y restricciones. |
| 3 | `03_InsertInitialData.sql` | Inserta los datos iniciales. |
| 4 | `04_CreateStoredProcedures.sql` | Crea los procedimientos almacenados. |

Al finalizar, la base de datos debe contener:

**Tablas**

- `dbo.ChannelConfiguration`
- `dbo.Companies`

**Stored Procedures**

- `dbo.usp_GetChannelConfiguration`

### 3.3 Estructura de ChannelConfiguration

La tabla `ChannelConfiguration` almacena los parámetros necesarios para configurar el comportamiento de cada canal.

| Columna | Descripción |
|---|---|
| ChannelConfigurationId | Identificador único de la configuración. |
| ChannelCode | Código del canal. |
| ServiceUrl | Dirección del servicio de compañías. |
| ApiKey | Clave de autenticación del canal. |
| ShowMenu | Indica si el menú debe mostrarse. |
| DestinationPage | Página de destino después de seleccionar una compañía. |
| IsActive | Indica si el canal está activo. |
| CreatedAt | Fecha de creación del registro. |
| UpdatedAt | Fecha de última actualización. |

La configuración inicial contiene dos canales:

| Canal | ShowMenu | DestinationPage | Estado |
|---|---:|---|---|
| AB | 1 | Destination.aspx | Activo |
| CJ | 0 | Destination.aspx | Activo |

### 3.4 Configuración de la conexión

Abrir el archivo `Web.config` y configurar la cadena de conexión según el entorno instalado.

**SQL Server Express:**

```xml
<connectionStrings>
    <add name="ESolutionsPayments"
         connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=ESolutionsPayments;Integrated Security=True;TrustServerCertificate=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

**SQL Server LocalDB:**

```xml
<connectionStrings>
    <add name="ESolutionsPayments"
         connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ESolutionsPayments;Integrated Security=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Utilizar únicamente la cadena correspondiente al entorno configurado.

---

## 4. Ejecución de la aplicación

El servicio REST y la interfaz ASP.NET Web Forms se encuentran alojados dentro del proyecto Web.

Por lo tanto, al ejecutar la aplicación web también estarán disponibles los endpoints de la API.

### 4.1 Abrir la solución

1. Abrir Visual Studio 2022.
2. Cargar el archivo `ESolutions.Payments.sln`.
3. Verificar que los tres proyectos estén cargados correctamente.
4. Restaurar los paquetes NuGet necesarios.
5. Compilar la solución.

### 4.2 Configurar el proyecto de inicio

Establecer el siguiente proyecto como Startup Project:

```text
ESolutions.Payments.Web
```

### 4.3 Verificar la dirección de la aplicación

Verificar la dirección HTTPS y el puerto configurado en el proyecto Web.

Los ejemplos de esta documentación utilizan:

```text
https://localhost:44352
```

Si Visual Studio asigna otro puerto, actualizar los valores de `ServiceUrl` en la tabla `ChannelConfiguration`.

### 4.4 Iniciar la aplicación

Ejecutar el proyecto Web utilizando IIS Express.

La aplicación estará disponible en:

```text
https://localhost:44352
```

El endpoint REST estará disponible en:

```http
GET https://localhost:44352/api/v1/companies?channel=AB
```

---

## 5. Uso de la aplicación

### 5.1 Canal AB

Acceder a la siguiente dirección:

```text
https://localhost:44352/Companies.aspx?canal=AB
```

Comportamiento esperado:

1. Se recupera la configuración del canal AB desde SQL Server.
2. Se utiliza el `ServiceUrl` y el `ApiKey` configurados.
3. Se consulta el servicio de compañías.
4. Se muestra el menú porque `ShowMenu = 1`.
5. El usuario puede seleccionar una compañía.
6. La compañía seleccionada se almacena en Session.
7. Se realiza la redirección hacia `Destination.aspx`.

### 5.2 Canal CJ

Acceder a:

```text
https://localhost:44352/Companies.aspx?canal=CJ
```

Comportamiento esperado:

1. Se recupera la configuración del canal CJ.
2. Se utiliza su respectivo API Key.
3. Se consulta el catálogo de compañías.
4. El menú permanece oculto porque `ShowMenu = 0`.
5. El usuario puede seleccionar una compañía.
6. Se realiza la redirección hacia la página configurada.

### 5.3 Página Destination

Después de seleccionar una compañía, la aplicación muestra:

- Código del canal seleccionado.
- Nombre de la compañía.
- Identificador de la compañía.
- Estado del flujo de pago.

Si la información requerida en Session no existe o ha expirado, la aplicación devuelve al usuario al listado de compañías.

La ejecución de pagos reales se encuentra fuera del alcance de esta demostración.

---

## 6. Documentación del servicio REST

### Endpoint

```http
GET /api/v1/companies?channel={channel}
```

### Encabezados de la solicitud

| Encabezado | Obligatorio | Descripción |
|---|---|---|
| X-Api-Key | Sí | API Key asociada al canal solicitado. |

### Ejemplo de solicitud

```http
GET https://localhost:44352/api/v1/companies?channel=AB
X-Api-Key: AB-DEMO-API-KEY
```

### Respuesta exitosa — HTTP 200

El servicio devuelve el catálogo de compañías disponibles.

```json
[
    {
        "Id": 1,
        "Name": "ANDA",
        "logoUrl": "https://example.com/anda.webp"
    },
    {
        "Id": 2,
        "Name": "CAESS",
        "logoUrl": "https://example.com/caess.webp"
    }
]
```

Los valores anteriores son ilustrativos y muestran la estructura esperada de la respuesta.

### Códigos HTTP

| Código | Descripción |
|---|---|
| 200 OK | Consulta de compañías realizada correctamente. |
| 400 Bad Request | El parámetro de canal es obligatorio o inválido. |
| 401 Unauthorized | La API Key no fue proporcionada o no es válida para el canal. |
| 404 Not Found | El canal solicitado no está configurado. |
| 500 Internal Server Error | Se produjo un error inesperado en el servidor. |

---

## 7. Pruebas con Postman

La solución incluye una colección de Postman denominada: Se encuentra en la carpeta Collection

```text
Payments_Postman_Collection.json
```

Esta colección permite validar los endpoints y comprobar el comportamiento de autenticación de los canales.

### 7.1 Importar la colección

1. Abrir Postman.
2. Seleccionar la opción **Import**.
3. Seleccionar el archivo JSON de la colección.
4. Abrir la colección importada.
5. Configurar las variables de la colección.

### 7.2 Variables de la colección

| Variable | Ejemplo | Descripción |
|---|---|---|
| baseUrl | https://localhost:44352 | Dirección base de la aplicación. |
| abApiKey | AB-DEMO-API-KEY | API Key del canal AB. |
| cjApiKey | CJ-DEMO-API-KEY | API Key del canal CJ. |
| invalidApiKey | INVALID-API-KEY | Clave inválida para las pruebas negativas. |

La variable `baseUrl` debe coincidir con el puerto HTTPS configurado en Visual Studio.

### 7.3 Escenarios de prueba

La colección contempla los siguientes escenarios:

| # | Escenario | Resultado esperado |
|---|---|---|
| 1 | Canal AB con API Key AB válida | 200 OK |
| 2 | Canal CJ con API Key CJ válida | 200 OK |
| 3 | Solicitud sin API Key | 401 Unauthorized |
| 4 | Solicitud con API Key inválida | 401 Unauthorized |
| 5 | Canal AB con API Key CJ | 401 Unauthorized |
| 6 | Canal CJ con API Key AB | 401 Unauthorized |
| 7 | Canal inexistente ZZ | 404 Not Found |
| 8 | Solicitud sin parámetro channel | 400 Bad Request |

### 7.4 Descripción de los escenarios

**Escenario 1 — Autenticación válida para AB**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=AB
X-Api-Key: {{abApiKey}}
```

Resultado esperado: HTTP 200 y listado de compañías.

**Escenario 2 — Autenticación válida para CJ**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=CJ
X-Api-Key: {{cjApiKey}}
```

Resultado esperado: HTTP 200 y listado de compañías.

**Escenario 3 — API Key no proporcionada**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=AB
```

Resultado esperado: HTTP 401.

**Escenario 4 — API Key inválida**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=AB
X-Api-Key: {{invalidApiKey}}
```

Resultado esperado: HTTP 401.

**Escenario 5 — Autenticación cruzada AB / CJ**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=AB
X-Api-Key: {{cjApiKey}}
```

Resultado esperado: HTTP 401.

La API Key del canal CJ no debe permitir consultar el canal AB.

**Escenario 6 — Autenticación cruzada CJ / AB**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=CJ
X-Api-Key: {{abApiKey}}
```

Resultado esperado: HTTP 401.

**Escenario 7 — Canal inexistente**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies?channel=ZZ
X-Api-Key: {{abApiKey}}
```

Resultado esperado: HTTP 404.

**Escenario 8 — Parámetro channel no proporcionado**

Solicitud:

```http
GET {{baseUrl}}/api/v1/companies
X-Api-Key: {{abApiKey}}
```

Resultado esperado: HTTP 400.

La API debe devolver un mensaje controlado indicando que el parámetro del canal es obligatorio.

---

## 8. Incorporación de un nuevo canal sin modificar código

Uno de los objetivos principales de la solución es permitir la incorporación de nuevos canales mediante configuración en la base de datos, sin necesidad de crear páginas, controladores o lógica específica para cada canal.

Para agregar un nuevo canal, basta con insertar un registro en `dbo.ChannelConfiguration`.

### Ejemplo: incorporación del canal DE

```sql
USE ESolutionsPayments;
GO

INSERT INTO dbo.ChannelConfiguration
(
    ChannelCode,
    ServiceUrl,
    ApiKey,
    ShowMenu,
    DestinationPage,
    IsActive
)
VALUES
(
    'DE',
    'https://localhost:44352/api/v1/companies',
    'DE-DEMO-API-KEY',
    1,
    'Destination.aspx',
    1
);
GO
```

### Verificar la configuración

```sql
EXEC dbo.usp_GetChannelConfiguration
    @ChannelCode = 'DE';
```

### Probar el nuevo canal

Acceder desde el navegador:

```text
https://localhost:44352/Companies.aspx?canal=DE
```

O utilizar Postman:

```http
GET https://localhost:44352/api/v1/companies?channel=DE
X-Api-Key: DE-DEMO-API-KEY
```

Comportamiento esperado:

- La aplicación recupera la configuración del nuevo canal.
- La API valida la API Key contra la configuración almacenada.
- Se obtiene el catálogo de compañías.
- El menú se muestra u oculta según `ShowMenu`.
- La navegación utiliza la página de destino configurada.

No es necesario crear un nuevo controlador, página, repositorio ni agregar condiciones específicas para el canal DE.

**Importante:** `DestinationPage` debe corresponder a una página permitida por la validación de navegación de la aplicación. Para este ejemplo se utiliza la página existente `Destination.aspx`.

---

## 9. Manejo de errores y registro de excepciones

La aplicación contempla el manejo de errores esperados durante la ejecución.

Entre ellos:

- Canal inválido o inexistente.
- Canal inactivo.
- API Key ausente o incorrecta.
- Servicio de compañías no disponible.
- Tiempo de espera agotado.
- Configuración incorrecta de la página de destino.
- Información de Session incompleta o expirada.

Los detalles técnicos de las excepciones se registran mediante el mecanismo de tracing de .NET.

Ejemplo:

```csharp
System.Diagnostics.Trace.TraceError(
    "Error retrieving companies: {0}",
    exception);
```

La interfaz muestra mensajes controlados al usuario, evitando exponer detalles internos de la aplicación.

---

## 10. Consideraciones de seguridad

- Las API Keys se recuperan desde la configuración almacenada en SQL Server.
- La API valida que la clave corresponda al canal solicitado.
- Las consultas a la base de datos utilizan parámetros.
- La página de destino se valida antes de realizar la redirección.
- La compañía seleccionada se almacena en Session.
- Las direcciones de los logos se validan antes de mostrarse.
- La comunicación local utiliza HTTPS.

Las API Keys incluidas en los scripts son credenciales de demostración y no deben utilizarse en producción.

En un entorno productivo, las credenciales deben almacenarse y administrarse de forma segura, evitando incluir secretos reales en el código fuente o en repositorios públicos.

---

## 11. Alcance y consideraciones

- La solución corresponde a una evaluación técnica que demuestra la selección configurable de compañías de servicios.
- El servicio REST está alojado dentro de la aplicación Web.
- SQL Server almacena la configuración de los canales.
- El catálogo de compañías se proporciona mediante la fuente de datos configurada para el servicio.
- La página de destino demuestra el flujo de selección y navegación.
- No se implementan pagos reales, transacciones financieras ni integraciones con proveedores externos de pago.

---


## 12. Conclusión

ESolutions Payments demuestra una arquitectura configurable para consultar y mostrar compañías de servicios mediante diferentes canales.

La separación en tres capas —Business, Data y Web— permite mantener las responsabilidades organizadas y facilita el mantenimiento y la evolución de la solución.

La configuración de canales almacenada en SQL Server permite incorporar nuevos canales sin modificar el código de la aplicación, mientras que la validación de API Key garantiza que cada canal utilice sus propias credenciales de acceso.
