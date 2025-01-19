# Distributed Caching in .NET with Redis

This project demonstrates how to implement distributed caching in a .NET application using Redis, along with a modular architecture for product management. It includes custom caching extensions, RESTful APIs, and database operations using Entity Framework Core.

---

## Key Features

- **Distributed Caching**:
  - Custom caching extensions for `IDistributedCache`.
  - Cache invalidation for consistency.
  - Efficient cache miss handling with `GetOrSetAsync`.
- **RESTful API**:
  - Endpoints for managing products with appropriate HTTP methods.
  - Validation and meaningful HTTP responses.
- **Database Integration**:
  - Uses Entity Framework Core for database operations.
  - SQL Server configuration with migration support.
- **Redis Integration**:
  - Configured to use `StackExchange.Redis` as a distributed cache provider.
  - Customizable connection settings.
- **Swagger UI**:
  - Integrated for API exploration and testing.

---

## Technologies Used

- **.NET 8**
- **Entity Framework Core** for database operations.
- **Redis** as a distributed cache provider.
- **Swagger/OpenAPI** for API documentation.
- **ASP.NET Core Web API** for creating RESTful endpoints.

---

## Project Structure

### **Controllers**

The `ProductsController` defines RESTful APIs for managing products:
- `GET /api/v1/products`: Fetch all products.
- `GET /api/v1/products/{id}`: Fetch a product by its ID.
- `POST /api/v1/products`: Create a new product.

### **Services**

The `ProductServiceDistributedCaching` service:
- Implements `IProductService` to handle business logic for products.
- Integrates distributed caching for efficient data retrieval and storage.
- Includes methods like:
  - `GetAll`: Retrieves all products with cache-first logic.
  - `Get`: Fetches a product by ID, caching results for subsequent requests.
  - `Add`: Adds a new product and invalidates the cache for consistency.

### **Extensions**

The `DistributedCacheExtensions` provide utility methods for:
- Setting cache entries (`SetAsync`).
- Retrieving cache entries with `TryGetValue`.
- Managing cache misses with `GetOrSetAsync`.

### **Models**

- **Entities**: Defines the `Product` entity.
- **DTOs**: Includes `ProductCreationDto` for validating input during product creation.

### **Program.cs**

The `Program.cs` file configures essential services:
- **Database**: Configures SQL Server with `AppDbContext`.
- **Distributed Caching**: Configures Redis as the distributed cache provider.
- **Swagger**: Adds API documentation for easier testing.
- **Dependency Injection**: Registers services and interfaces.

---

## Installation

### Prerequisites

- **.NET 8 SDK**
- **Redis Server** 
- **SQL Server**

### Setup

1. **Clone the repository**:
   ```bash
   git@github.com:MirzaJalal/DistributedCaching.git
   
