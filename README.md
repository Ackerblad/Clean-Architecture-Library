# Clean-Architecture-Library

This project is a Clean Architecture implementation for managing users, books and authors. It demonstrates the use of CQRS, Test-Driven Development and a layered structure to ensure maintainability, scalability and testability.

### Architecture
- **Domain Layer**
- **Application Layer**
- **Infrastructure Layer**
- **Presentation Layer**
  
### Technical 
- **CQRS**
- **Entity Framework Core**
- **Generic Repository**
- **Operation Result Pattern**
- **DTOs (Data Transfer Objects)**
- **Validators**
- **AutoMapper**
- **Password Hashing**
- **JWT Authentication**
- **Caching**
- **Logging**

### Testing
- **Unit Tests**
- **Integration Tests**

## 

### Configure the Connection String
This project uses `dotnet user-secrets` to store the connection string.

#### Option A: Use `dotnet user-secrets`
1. Initialize user secrets in the API project:
   ```bash
   dotnet user-secrets init
   ```
2. Add your connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YourServerName;Database=YourDatabaseName;Trusted_Connection=True;"
   ```

#### Option B: Use `appsettings.json`
Open `appsettings.json` and replace `YourConnectionStringHere` with your actual connection string.
