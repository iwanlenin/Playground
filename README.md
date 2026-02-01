# PlayGround App

A modern .NET MAUI application demonstrating a complete MVVM architecture with receipt management and SQLite database integration.

## ?? Project Overview

**PlayGround App** is a .NET MAUI (Multi-platform App UI) application built with .NET 10, showcasing best practices in:

- **MVVM Pattern**: Complete Model-View-ViewModel implementation with proper separation of concerns
- **Dependency Injection**: Service-based architecture using Microsoft's DI container
- **Data Persistence**: SQLite database integration with async operations
- **Property Change Notifications**: Automatic UI updates using `INotifyPropertyChanged`

## ?? Key Features

### 1. Counter Application
- Simple counter functionality demonstrating MVVM pattern
- Increment button with automatic text updates
- Service-based state management

### 2. Receipt Management
- Receipt CRUD operations (Create, Read, Update, Delete)
- Receipt items with one-to-many relationships
- Transactional database operations
- Async/await patterns for database access

## ??? Architecture

```
PlayGroundApp/
??? Models/                    # Data models
?   ??? Receipt.cs            # Receipt entity
?   ??? ReceiptItem.cs        # Receipt item entity
?   ??? CounterModel.cs       # Counter model
??? Views/                    # XAML pages
?   ??? MainPage.xaml         # Main application view
??? ViewModels/               # View models
?   ??? BaseViewModel.cs      # Base class with INotifyPropertyChanged
?   ??? MainViewModel.cs      # Main page view model
??? Services/                 # Business logic
?   ??? ICounterService.cs    # Counter service interface
?   ??? CounterService.cs     # Counter service implementation
?   ??? DatabaseService.cs    # Database operations
??? Resources/                # App resources
```

## ?? Technology Stack

- **.NET 10** - Latest .NET framework
- **.NET MAUI** - Cross-platform UI framework
- **SQLite** - Lightweight database
- **MVVM Pattern** - UI architecture
- **Dependency Injection** - Service management

## ?? Prerequisites

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Visual Studio 2022** (recommended) or **Visual Studio Code**
- **Git** - For version control

## ?? Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/iwanlenin/Playground.git
cd Playground
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Application

```bash
# Run on Windows
dotnet run --project PlayGroundApp/PlayGroundApp.csproj -f net10.0-windows

# Run on Android
dotnet run --project PlayGroundApp/PlayGroundApp.csproj -f net10.0-android

# Run on iOS
dotnet run --project PlayGroundApp/PlayGroundApp.csproj -f net10.0-ios

# Run on macOS
dotnet run --project PlayGroundApp/PlayGroundApp.csproj -f net10.0-maccatalyst
```

## ?? Project Structure

### Models
- **Receipt**: Represents a purchase receipt with store name, purchase date, and image
- **ReceiptItem**: Individual items on a receipt with product details and quantity
- **CounterModel**: Simple counter value holder

### Views
- **MainPage**: Main application view with counter demonstration

### ViewModels
- **BaseViewModel**: Provides `INotifyPropertyChanged` implementation for all view models
- **MainViewModel**: Manages counter state and user interactions

### Services
- **ICounterService**: Interface defining counter operations
- **CounterService**: Implements counter logic
- **DatabaseService**: Handles all database operations (CRUD) with async support

## ?? Database

### SQLite Tables

#### Receipts Table
```sql
CREATE TABLE Receipts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StoreName TEXT,
    PurchaseDate DATETIME,
    ImagePath TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

#### ReceiptItems Table
```sql
CREATE TABLE ReceiptItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ReceiptId INTEGER NOT NULL,
    ProductName TEXT,
    Price DECIMAL,
    Category TEXT,
    Quantity INTEGER DEFAULT 1,
    FOREIGN KEY (ReceiptId) REFERENCES Receipts(Id)
);
```

## ?? Development

### Adding New Features

1. **Create a Model** in `Models/` folder
2. **Implement Database Service** in `Services/` folder
3. **Create ViewModel** in `ViewModels/` folder
4. **Design View** in `Views/` folder with XAML

### Code Style

- Use `this.` prefix for private fields (no underscore convention)
- Follow XML documentation comments for all public members
- Use `async/await` for all I/O operations
- Implement `INotifyPropertyChanged` in all view models

## ?? Testing

### Build in Debug Mode
```bash
dotnet build -c Debug
```

### Build in Release Mode
```bash
dotnet build -c Release
```

## ?? Documentation

- **README.md** - This file (setup and overview)
- **CODE_DOCUMENTATION.md** - Detailed architecture and code documentation
- **Generated API Docs** - Automatically generated when building in Release mode

Documentation is automatically generated during Release builds using DocFX.

## ?? Release Build

To create a release build with automatic documentation generation:

```bash
dotnet build -c Release
```

This will:
1. Compile the project in Release mode
2. Generate API documentation
3. Create markdown documentation
4. Output documentation to `docs/` folder

## ?? Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the LICENSE file for details.

## ????? Author

[iwanlenin](https://github.com/iwanlenin)

## ?? Links

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [MAUI Documentation](https://learn.microsoft.com/maui/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [MVVM Pattern](https://learn.microsoft.com/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)

## ?? Support

For issues and questions, please open an issue on [GitHub Issues](https://github.com/iwanlenin/Playground/issues).

---

**Last Updated**: 2025-01-13
**Version**: 1.0.0
