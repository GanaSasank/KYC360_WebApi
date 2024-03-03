# KYC360 ASP.NET Core Web API Project

## Overview

This is an ASP.NET Core Web API project named KYC360, designed to handle Know Your Customer (KYC) data. The project includes CRUD operations, searching, and filtering capabilities for entities.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed
- MySQL server up and running
- A tool for making HTTP requests (e.g., [Postman](https://www.postman.com/))

## Getting Started

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/KYC360.git
    ```

2. Navigate to the project directory:

    ```bash
    cd KYC360
    ```

3. Update the database connection string:

    Open `appsettings.json` and replace the MySQL connection string in the `"DefaultConnection"` section with your database details.

4. Apply Entity Framework migrations:

    ```bash
    dotnet ef database update
    ```

5. Run the API:

    ```bash
    dotnet run
    ```

    The API will be accessible at `https://localhost:44304`.

## API Endpoints

### Retrieve all entities:

```http
## API Endpoints
 Retrieve a single entity by ID:
GET https://localhost:44304/api/Entity/{id}
GET https://localhost:44304/api/Entity/Search?query={searchQuery}
PUT https://localhost:44304/api/Entity/{id}
POST https://localhost:44304/api/Entity
DELETE https://localhost:44304/api/Entity/{id}




