# System Configuration API

A robust, RESTful API service designed to manage dynamic application settings and feature flags. This project utilizes **.NET 10** and follows **Clean Architecture** principles to ensure scalability, maintainability, and separation of concerns.

## 🚀 Overview

The System Configuration API serves as a centralized source of truth for system behaviors. It allows administrators or other services to retrieve and modify configuration values and toggle feature flags at runtime without redeploying the application.

### Key Capabilities
* **Feature Flag Management:** Create, update, toggle, and delete feature flags to control software release cycles.
* **Application Settings:** Manage typed configuration keys (String, Number, Boolean, JSON).
* **High Performance:** Implements `IMemoryCache` with sliding expiration strategies to reduce database load for high-read endpoints.
* **Observability:** Integrated structured logging using **Serilog**, writing logs to both Console and SQL Server.
* **Versioning:** API versioning support (V1) to ensure backward compatibility.

## 🛠 Technology Stack

* **Framework:** .NET 10.0
* **Language:** C#
* **Database:** SQL Server
* **ORM:** Entity Framework Core 10.0.1
* **Logging:** Serilog
* **Documentation:** Swagger

## 🏗 Architecture

The solution adheres to **Clean Architecture** and is organized into four projects:

1.  **API (`PBC.SystemConfiguration.API`):** The entry point. Contains Controllers, Middlewares, and DI configurations.
2.  **Application (`PBC.SystemConfiguration.Application`):** Contains business logic, DTOs, Service Interfaces, and Validation.
3.  **Domain (`PBC.SystemConfiguration.Domain`):** Contains Entities, Enums, and Repository Interfaces. Dependent on nothing.
4.  **Infrastructure (`PBC.SystemConfiguration.Infrastructure`):** Implements Repositories, DB Context, and Migrations.

## ⚙️ Getting Started

### Prerequisites
* .NET 10 SDK
* SQL Server

### Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/Productive-Build-Cycle/System-Configuration.git
    cd system-configuration
    ```

2.  **Configure the Database:**
    Update the `ConnectionStrings` in `src/PBC.SystemConfiguration.API/appsettings.Development.json` to point to your SQL Server instance.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=SystemConfiguration;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```

3.  **Apply Migrations:**
    Update the database. This will create the `AppSettings` and `FeatureFlags` tables and seed initial data.
    ```bash
    dotnet ef database update --project src/PBC.SystemConfiguration.Infrastructure --startup-project src/PBC.SystemConfiguration.API
    ```
    *Note: The migration seeds a default feature flag named "New UI" (Disabled).*

4.  **Run the Application:**
    ```bash
    dotnet run
    ```
    The API will launch on:
    * HTTP: `http://localhost:5026`
    * HTTPS: `https://localhost:7281`

## 📖 API Documentation

The project includes **Swagger/OpenAPI** integration. Once the application is running in the `Development` environment, you can access the interactive API documentation at:
    `http://localhost:5026/swagger/index.html`

### Main Endpoints

#### 🚩 Feature Flags
* `GET /api/v1/FeatureFlag`: Retrieve all flags.
* `GET /api/v1/FeatureFlag/{name}`: Retrieve a specific flag by name (Cached).
* `POST /api/v1/FeatureFlag`: Create a new flag.
* `PATCH /api/v1/FeatureFlag/{name}`: Quickly toggle `IsEnabled` status.
* `PUT /api/v1/FeatureFlag/{id}`: Update flag details.
* `DELETE /api/v1/FeatureFlag/{id}`: Remove a flag.

#### ⚙️ App Settings
* `GET /api/v1/AppSettings`: Retrieve all settings.
* `GET /api/v1/AppSettings/{key}`: Retrieve a setting by key (Cached).
* `POST /api/v1/AppSettings`: Create a new setting.
* `PUT /api/v1/AppSettings/{id}`: Update an existing setting.
* `DELETE /api/v1/AppSettings/{id}`: Delete a setting.

## 🔍 Observability

### Logging
The application uses a custom **Request Body Logging Middleware** to capture payload details for `POST`, `PUT`, and `PATCH` requests.

Logs are written to the `Logs` table in your SQL Server database if the connection is successful. The log schema includes:
* `LogEvent`: Full JSON log event.
* `SourceContext`: The class where the log originated.
* `RequestBody`: The captured HTTP request body.

### Error Handling
A global `ExceptionHandlerMiddleware` intercepts exceptions:
* **Domain Exceptions** (e.g., `ObjectNotFound`, `ObjectAlreadyExists`) return standardized 4xx responses.
* **Unhandled Exceptions** return a generic 500 response to prevent leaking sensitive stack traces.
