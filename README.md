
# Direct Mail - Sample
 
It showcases the best practices of integrating Customer's Canvas Hub with a Nuxt 3 (Vue 3) + .NET8-based application.
 
You can run the repo using one of two methods:
1. Containerized (with Docker) - recommended, the easiest and fastest way.
2. Locally (without Docker) - more involved and slower to set up, but might be more useful for fiddling around with the source code.
 
## Prerequisites
 
### For containerized build
Install:
1. [Docker Desktop](https://www.docker.com/products/docker-desktop/)
 
### For local build
Install:
1. [Node.js 20+](https://nodejs.org/en)
2. [.Net8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
3. [PostgreSql](https://www.postgresql.org/download/)


## Getting Started
### Customer's Canvas tenant

For integration with Customer's Canvas, you need:

1. An active tenant.
2. An integration of the **Custom** type. To create one, navigate to **Settings** > **Integrations**.

3. An **External App** created for the tenant. To create one, navigate to **Settings** > **External Apps**. Grant **Full** permissions to the resources. For authentication, you will need **Client ID** and **Secret key**.

For more details about custom integrations, you can refer to the [documentation](https://customerscanvas.com/dev/backoffice/storefront/creating-custom-integration.html).

## Configuring the Application

You can download the application from GitHub:

You can run the application in Docker or build the code and run locally.

### Docker settings

Before running the application in Docker, specify the database and Customer's Canvas parameters first.

In **docker-compose.yml** file:

- Postgres settings: **email** and **password**

    ```text
    11. - 'POSTGRES_PASSWORD=<POSTGRES_PASSWORD>' 
    28.    PGADMIN_DEFAULT_EMAIL: '<YOUR_EMAIL>'
    29.    PGADMIN_DEFAULT_PASSWORD: '<MASTER_PG_ADMIN_PASSWORD>'
    ```

- Database connection: **UserId** and **Password**

    ```text
    45. -  ConnectionStrings__DefaultConnection= Server=postgres;Port=5432;UserId=postgres;Password=<POSTGRES_PASSWORD>;Database=AurigmaDirectMailDb;
    ```

- **BaseUrl** of Customer's Canvas API, depending on your environment, <https://api.customerscanvashub.com> or <https://api.eu.customerscanvashub.com>

    ```text
    47. - CustomersCanvas__BaseUrl=<BASE_CUSTOMERS_CANVAS_API_URL>
    ```

- **TenantID** can be found in BackOffice under **Settings** > **Tenant**

    ```text
    48. - CustomersCanvas__TenantId=<YOUR_TENANT_ID_FROM_CUSTOMERS_CANVAS>
    ```

- **StorefrontID** can be found in BackOffice under **Settings** > **Integrations**

    ```text
    49. - CustomersCanvas__StorefrontId=<YOUR_STOREFRONT_ID_FROM_CUSTOMERS_CANVAS>
    ```

- **ClientID** and **Client Secret** can be found in BackOffice under **Settings** > **External Apps**. Click an app to see its details.

    ```text
    50. - CustomersCanvas__ClientId=<YOUR_EXTERNAL_APP_CLIENT_ID_FROM_CUSTOMERS_CANVAS>
    51. - CustomersCanvas__ClientSecret=<YOUR_EXTERNAL_APP_CLIENT_SECRET_FROM_CUSTOMERS_CANVAS>
    ```

- **Base API URL**, where the service will run, for example, <http://localhost:8092>

    ```text
    66. - BASE_URL=<BASE_BACKEND_API_URL_FROM_GLOBAL_WEB>
    ```

### Backend Settings

Configure the backend in the file **/src/Aurigma.DirectMail.Sample.WebHost/appsettings.json**:

```json
"DefaultConnection": "<Your connection string to postgreSql server>",
"CustomersCanvas": {
    "ApiGatewayUrl": "<Base customer's canvas api url>",
    "TenantId": "<Your tenant id from customer's canvas>",
    "StorefrontId": "<Your storefront id from customer's canvas, which you can create in Settings->Integrations>",
    "ClientId": "<Your external app client id from customer's canvas, which you can create in Settings->External Apps>",
    "ClientSecret": "<Your external app client secret from customer's canvas, which you can create in Settings->External Apps>"
}
```


## Running the Application

### Launching Docker

Before launching Docker, you can delete previously created containers and project instances. 

To run docker-compose, use the command with the modifiers:

`docker-compose up --build --force-recreate`

Then, go to <https://localhost:8091> to see the frontend.

### Building on your own

To build the backend:

1. Open the **Backend** folder (from project's root folder):

   `cd ./Backend`

2. Install the dependencies:

   `<a name="_int_2dhmubl3"></a>dotnet restore "./src/Aurigma.DirectMail.Sample.WebHost/Aurigma.DirectMail.Sample.WebHost.csproj"`

3. Build the project:

   `<a name="_int_ssb8jh61"></a>dotnet build "./src/Aurigma.DirectMail.Sample.WebHost/Aurigma.DirectMail.Sample.WebHost.csproj" -c Release`

4. Go to the build folder:

   `cd ./src/Aurigma.DirectMail.Sample.WebHost/bin/net8.0`

5. Start the project:

   `dotnet ./Aurigma.DirectMail.Sample.WebHost.dll`

To build the frontend:

1. Open the **Frontend** folder (from project's root folder):

   `cd ./Frontend`

2. Install the dependencies:

   `npm install`

3. Start the development server on <http://localhost:3000>:

   `npm run dev`

4. Build the application for production:

   `npm run build`


## Documentation

Click the link here to be brought to all of the documentation: [Direct Mail App Wiki](https://github.com/aurigma/direct-mail-app-sample/wiki)
