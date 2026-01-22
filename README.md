# Order Processing System

## Project Overview

This is a production-ready order processing system built with .NET 8. The application demonstrates modern software engineering practices including dependency injection, asynchronous programming, thread safety, and comprehensive error handling. The system processes orders in parallel, validates input, logs operations, and sends notifications - all while maintaining clean architecture principles.

The solution consists of two projects:
- **OrderPrecessing** - Main console application
- **OrderProcessing.Tests** - Unit test project using xUnit and Moq

## How to Run

### Prerequisites
- .NET 8.0 SDK or later

### Build the Solution
```bash
dotnet build
```

### Run the Application
```bash
dotnet run --project OrderPrecessing.csproj
```

### Run Tests
```bash
dotnet test
```

## Architecture Diagram

```mermaid
sequenceDiagram
    participant Main
    participant OrderService
    participant OrderValidator
    participant Logger
    participant Repository
    participant NotificationService
    
    Main->>OrderService: ProcessOrderAsync(orderId)
    OrderService->>Logger: LogInfo("Starting to process...")
    
    alt Order ID is invalid
        OrderService->>OrderValidator: IsValid(orderId)
        OrderValidator-->>OrderService: false
        OrderService->>Logger: LogError("Invalid order ID")
        OrderService-->>Main: return
    else Order ID is valid
        OrderService->>OrderValidator: IsValid(orderId)
        OrderValidator-->>OrderService: true
        
        alt Order found
            OrderService->>Repository: GetOrderAsync(orderId)
            Repository->>Repository: Task.Delay(100)
            Repository-->>OrderService: productName
            OrderService->>Logger: LogInfo("Order processed successfully")
            OrderService->>NotificationService: SendAsync(message)
            NotificationService-->>OrderService: Task.CompletedTask
        else Order not found
            OrderService->>Repository: GetOrderAsync(orderId)
            Repository-->>OrderService: KeyNotFoundException
            OrderService->>Logger: LogError("Failed to process order")
        end
    end
```

## List of Completed Bonus Tasks

### ✅ Bonus Tasks

**1. Asynchronous Processing**
- Changed `ProcessOrder` to `ProcessOrderAsync(int orderId)` returning `Task`
- Simulated delay in repository with `Task.Delay(100)` to mimic real async work
- Used `await` and `Task.WhenAll` in Main for parallel execution
- All async operations properly awaited throughout the call chain

**2. Add Order (CRUD)**
- Added `AddOrderAsync(Order order)` method to `IOrderRepository`
- Ensured thread-safe insert using `ConcurrentDictionary.TryAdd`
- Rejects duplicates with `InvalidOperationException`
- Called from a new `Task` in Main to demonstrate concurrent operations

**3. IOrderValidator**
- Created `IOrderValidator` interface with `bool IsValid(int orderId)` method
- Implemented `OrderValidator` with simple positive ID validation
- Injected into `OrderService` and validates before repository call
- Prevents unnecessary repository access for invalid IDs

**4. Unit Tests**
- Added xUnit test project `OrderProcessing.Tests`
- Written 3+ comprehensive tests:
  - **Happy Path**: Valid order ID processes successfully, calls repository and notification
  - **Invalid ID**: Invalid order ID doesn't call repository, logs error
  - **Not Found**: `KeyNotFoundException` is caught and logged properly
- All tests use Moq to mock `ILogger` and `IOrderRepository`
- Tests verify correct method calls and exception handling

**5. Configuration via appsettings.json**
- Added `appsettings.json` with `LogLevel` configuration
- `ConsoleLogger` reads configuration via `IConfiguration`
- When `LogLevel` is set to "Error", Info logs are skipped
- Configuration properly loaded in `ServiceContainer`

**6. Notification Service**
- Created `INotificationService` interface with `SendAsync(string message)` method
- Implemented `EmailNotificationService` that logs notifications to console
- Injected into `OrderService` and called after successful order processing
- Sends notification with order details (ID and product name)

## Key Features

- **Thread Safety**: All repository operations use `ConcurrentDictionary` for safe concurrent access
- **Error Handling**: Comprehensive exception handling with proper logging
- **Dependency Injection**: Clean architecture with all dependencies injected
- **Async/Await**: Full asynchronous support with proper `Task` usage
- **Configuration**: JSON-based configuration for log levels
- **Testing**: Complete unit test coverage with mocking
- **SOLID Principles**: Single responsibility, dependency inversion, and interface segregation

## Project Structure

```
OrderPrecessing/
├── Order.cs                      # Domain model
├── IOrderRepository.cs           # Repository interface
├── IOrderService.cs              # Service interface
├── ILogger.cs                    # Logger interface
├── IOrderValidator.cs            # Validator interface
├── INotificationService.cs      # Notification interface
├── InMemoryOrderRepository.cs   # Thread-safe repository implementation
├── OrderService.cs               # Business logic implementation
├── ConsoleLogger.cs              # Logger with appsettings.json support
├── OrderValidator.cs             # Input validation
├── EmailNotificationService.cs  # Notification implementation
├── ServiceContainer.cs           # DI configuration
├── Program.cs                    # Entry point with parallel processing
├── appsettings.json              # Configuration file
└── OrderProcessing.Tests/       # Unit test project
    └── OrderServiceTests.cs      # xUnit tests with Moq
```
