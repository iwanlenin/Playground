# Code Documentation

Detailed documentation of the PlayGround App architecture, design patterns, and implementation details.

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Design Patterns](#design-patterns)
3. [Models](#models)
4. [Services](#services)
5. [ViewModels](#viewmodels)
6. [Views](#views)
7. [Database Design](#database-design)
8. [Dependency Injection](#dependency-injection)
9. [Data Flow](#data-flow)
10. [Common Patterns](#common-patterns)

---

## Architecture Overview

The PlayGround App follows the **MVVM (Model-View-ViewModel)** architecture pattern with clear separation of concerns:

```
???????????????????????????????????????????????????????????????????
?                          USER INTERFACE                          ?
?                    (Views - XAML + Code-Behind)                 ?
???????????????????????????????????????????????????????????????????
                         ? Binding
                         ? Commands
                         ?
???????????????????????????????????????????????????????????????????
?                      VIEW MODELS                                  ?
?        (Business Logic + State Management)                       ?
?                  BaseViewModel                                   ?
?                  MainViewModel                                   ?
???????????????????????????????????????????????????????????????????
                         ? Calls
                         ? Dependency Injection
                         ?
???????????????????????????????????????????????????????????????????
?                        SERVICES                                   ?
?         (Business Logic Implementation)                          ?
?                  ICounterService                                 ?
?                  CounterService                                  ?
?                  DatabaseService                                 ?
???????????????????????????????????????????????????????????????????
                         ? Uses
                         ?
???????????????????????????????????????????????????????????????????
?                         MODELS                                    ?
?            (Data Structures + Domain Objects)                    ?
?                  Receipt                                         ?
?                  ReceiptItem                                     ?
?                  CounterModel                                    ?
???????????????????????????????????????????????????????????????????
                         ? Persists
                         ?
???????????????????????????????????????????????????????????????????
?                       DATABASE                                    ?
?                     (SQLite)                                     ?
???????????????????????????????????????????????????????????????????
```

---

## Design Patterns

### 1. MVVM (Model-View-ViewModel)

**Purpose**: Separate UI logic from business logic and data access.

**Implementation**:
- **Model**: Contains data structures (Receipt, ReceiptItem, CounterModel)
- **ViewModel**: Contains state and commands (MainViewModel extends BaseViewModel)
- **View**: XAML markup with data bindings

**Benefits**:
- Testability: ViewModels can be tested without UI
- Maintainability: Clear separation of concerns
- Reusability: ViewModels can be reused with different views

### 2. Dependency Injection (DI)

**Purpose**: Manage object dependencies and promote loose coupling.

**Implementation**:
```csharp
// In MauiProgram.cs
builder.Services.AddSingleton<ICounterService, CounterService>();
builder.Services.AddSingleton<MainViewModel>();
```

**Benefits**:
- Loose coupling between classes
- Easy testing with mock implementations
- Centralized service configuration

### 3. Repository Pattern (via DatabaseService)

**Purpose**: Abstract database access logic.

**Implementation**:
- `DatabaseService` handles all CRUD operations
- Services don't know about SQLite implementation details
- Easy to switch database providers

### 4. Service Locator Pattern

**Purpose**: Resolve dependencies at runtime.

**Implementation**:
```csharp
// In MainPage.xaml.cs
var vm = MauiProgram.Services?.GetService<MainViewModel>();
BindingContext = vm;
```

---

## Models

### Receipt Model

**Location**: `PlayGroundApp/Models/Receipt.cs`

**Purpose**: Represents a purchase receipt entity.

**Properties**:
- `Id` (int): Primary key, auto-incremented
- `StoreName` (string): Name of the store
- `PurchaseDate` (DateTime): When the purchase was made
- `ImagePath` (string): Path to receipt image
- `CreatedAt` (DateTime): When record was created (UTC)
- `Items` (List<ReceiptItem>): Navigation property for child items

**Database Table**: `Receipts`

**Example Usage**:
```csharp
var receipt = new Receipt
{
    StoreName = "SuperMart",
    PurchaseDate = DateTime.Now,
    ImagePath = "/images/receipt.jpg",
    CreatedAt = DateTime.UtcNow
};
```

### ReceiptItem Model

**Location**: `PlayGroundApp/Models/ReceiptItem.cs`

**Purpose**: Represents an individual item on a receipt.

**Properties**:
- `Id` (int): Primary key, auto-incremented
- `ReceiptId` (int): Foreign key to Receipt (indexed)
- `ProductName` (string): Name of the product
- `Price` (decimal): Price of the product
- `Category` (string): Product category
- `Quantity` (int): Quantity purchased (default: 1)
- `Receipt` (Receipt): Navigation property back to parent

**Database Table**: `ReceiptItems`

**Relationships**:
- Many-to-One with Receipt (ReceiptId foreign key)

**Example Usage**:
```csharp
var item = new ReceiptItem
{
    ReceiptId = 1,
    ProductName = "Milk",
    Price = 3.99m,
    Category = "Dairy",
    Quantity = 2
};
```

### CounterModel

**Location**: `PlayGroundApp/Models/CounterModel.cs`

**Purpose**: Simple model holding counter value.

**Properties**:
- `Count` (int): Current counter value

**Example Usage**:
```csharp
var counter = new CounterModel { Count = 5 };
```

---

## Services

### ICounterService Interface

**Location**: `PlayGroundApp/Services/ICounterService.cs`

**Purpose**: Defines contract for counter operations.

**Methods**:
- `int GetCount()`: Returns current count
- `void IncrementCount()`: Increments counter by 1

### CounterService Implementation

**Location**: `PlayGroundApp/Services/CounterService.cs`

**Purpose**: Implements counter business logic.

**Key Features**:
- Manages internal `CounterModel` instance
- Handles counter increment logic
- Single responsibility: only counter operations

**Example Usage**:
```csharp
var counterService = new CounterService();
counterService.IncrementCount();
int count = counterService.GetCount(); // 1
```

### DatabaseService

**Location**: `PlayGroundApp/Services/DatabaseService.cs`

**Purpose**: Manages all database operations for Receipt and ReceiptItem.

**Key Features**:
- Async/await operations for performance
- Transactional support for data consistency
- CRUD operations for both entities
- One-to-many relationship management

**Methods**:

#### Receipt Operations

| Method | Purpose |
|--------|---------|
| `GetReceiptsAsync()` | Get all receipts ordered by creation date |
| `GetReceiptAsync(id)` | Get single receipt by ID |
| `GetReceiptWithItemsAsync(id)` | Get receipt with all items populated |
| `SaveReceiptAsync(receipt)` | Insert new or update existing receipt |
| `DeleteReceiptAsync(id)` | Delete receipt by ID |

#### ReceiptItem Operations

| Method | Purpose |
|--------|---------|
| `GetReceiptItemsAsync(receiptId)` | Get all items for a receipt |
| `SaveReceiptItemAsync(item)` | Insert new or update existing item |
| `DeleteReceiptItemAsync(id)` | Delete item by ID |

#### Transactional Operations

| Method | Purpose |
|--------|---------|
| `SaveReceiptWithItemsAsync(receipt, items)` | Save receipt and items atomically |

**Example Usage**:
```csharp
var dbService = new DatabaseService(databasePath);

// Get all receipts
var receipts = await dbService.GetReceiptsAsync();

// Save receipt with items (transactional)
var receipt = new Receipt { StoreName = "Store" };
var items = new List<ReceiptItem> 
{ 
    new ReceiptItem { ProductName = "Item1", Price = 10m }
};
await dbService.SaveReceiptWithItemsAsync(receipt, items);

// Get receipt with items
var fullReceipt = await dbService.GetReceiptWithItemsAsync(1);
```

---

## ViewModels

### BaseViewModel

**Location**: `PlayGroundApp/ViewModels/BaseViewModel.cs`

**Purpose**: Base class for all ViewModels implementing `INotifyPropertyChanged`.

**Key Features**:
- Property change notifications for UI updates
- Helper methods for property management
- Automatic caller tracking using `[CallerMemberName]`

**Methods**:
- `SetProperty<T>(ref T storage, T value)`: Sets property and raises notification
- `OnPropertyChanged(propertyName)`: Manually raises PropertyChanged event

**Example Usage**:
```csharp
public class MyViewModel : BaseViewModel
{
    private string title;
    
    public string Title
    {
        get => this.title;
        set => SetProperty(ref this.title, value);
    }
}
```

**How It Works**:
1. `SetProperty` compares old and new values
2. If different, updates backing field
3. Calls `OnPropertyChanged` to notify UI
4. UI bindings automatically update

### MainViewModel

**Location**: `PlayGroundApp/ViewModels/MainViewModel.cs`

**Purpose**: ViewModel for the main counter application.

**Key Features**:
- Manages counter state
- Provides commands for user actions
- Updates UI text based on counter value
- Uses dependency injection for services

**Properties**:
- `CounterText` (string): Display text for button
- `IncrementCommand` (ICommand): Command for incrementing counter

**Key Methods**:
- `OnIncrement()`: Executes when counter button is clicked
- `UpdateCounterText()`: Updates display text based on count

**Data Flow**:
```
User clicks button
         ?
IncrementCommand executes
         ?
OnIncrement() called
         ?
CounterService.IncrementCount()
         ?
UpdateCounterText() called
         ?
CounterText property updated
         ?
UI automatically updates
```

**Example Implementation**:
```csharp
public MainViewModel(ICounterService counterService)
{
    this.counterService = counterService;
    IncrementCommand = new Command(OnIncrement);
    UpdateCounterText();
}
```

---

## Views

### MainPage

**Location**: `PlayGroundApp/Views/MainPage.xaml` and `PlayGroundApp/Views/MainPage.xaml.cs`

**Purpose**: Main application user interface.

**XAML Structure**:
```xml
<ContentPage>
    <ScrollView>
        <VerticalStackLayout>
            <Image /> <!-- App logo -->
            <Label /> <!-- Title -->
            <Label /> <!-- Welcome text -->
            <Button Command="{Binding IncrementCommand}" 
                    Text="{Binding CounterText}" /> <!-- Counter button -->
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

**Code-Behind**:
- Resolves ViewModel from DI container
- Sets BindingContext for data binding
- No business logic (follows MVVM)

**Bindings**:
- `Button.Text` ? `MainViewModel.CounterText`
- `Button.Command` ? `MainViewModel.IncrementCommand`

---

## Database Design

### Schema

#### Receipts Table
```sql
CREATE TABLE Receipts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StoreName TEXT,
    PurchaseDate DATETIME,
    ImagePath TEXT,
    CreatedAt DATETIME
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
    Quantity INTEGER
);

CREATE INDEX idx_ReceiptItems_ReceiptId ON ReceiptItems(ReceiptId);
```

### Relationships

**One-to-Many**: Receipt ? ReceiptItems
- One receipt can have multiple items
- Foreign key: `ReceiptItem.ReceiptId` ? `Receipt.Id`
- Index on `ReceiptId` for query performance

---

## Dependency Injection

### Configuration

**Location**: `PlayGroundApp/MauiProgram.cs`

**Registered Services**:
```csharp
builder.Services.AddSingleton<ICounterService, CounterService>();
builder.Services.AddSingleton<MainViewModel>();
```

**Service Lifetime**:
- `Singleton`: Single instance for application lifetime
- `Transient`: New instance each time
- `Scoped`: Single instance per scope

### Usage

**In Views**:
```csharp
var vm = MauiProgram.Services?.GetService<MainViewModel>();
BindingContext = vm;
```

**In Services**:
```csharp
public MainViewModel(ICounterService counterService)
{
    this.counterService = counterService;
}
```

---

## Data Flow

### Counter Example Flow

```
User Interface
    ?
[User clicks button]
    ?
MainPage.xaml button with Command binding
    ?
MainViewModel.IncrementCommand executes
    ?
MainViewModel.OnIncrement()
    ?
CounterService.IncrementCount()
    ?
CounterModel.Count++
    ?
UpdateCounterText()
    ?
CounterText property changed
    ?
PropertyChanged event fired
    ?
Binding updates
    ?
UI refreshes with new text
```

### Database Save Flow

```
ViewModel creates Receipt + Items
    ?
DatabaseService.SaveReceiptWithItemsAsync()
    ?
Begin Transaction
    ?
Insert Receipt ? get ID
    ?
Set ReceiptId on all Items
    ?
Insert all Items
    ?
Commit Transaction
    ?
Success or Rollback
```

---

## Common Patterns

### Property with Notification

```csharp
private string title;

public string Title
{
    get => this.title;
    set => SetProperty(ref this.title, value);
}
```

### Command Binding

```csharp
public ICommand MyCommand { get; }

public MyViewModel()
{
    MyCommand = new Command(OnMyCommand);
}

private void OnMyCommand()
{
    // Handle command execution
}
```

### Async Operation

```csharp
public async Task LoadDataAsync()
{
    var data = await databaseService.GetDataAsync();
    // Update UI
}
```

### Transactional Operation

```csharp
public async Task SaveWithRelationsAsync()
{
    await databaseService.SaveReceiptWithItemsAsync(receipt, items);
}
```

---

## Best Practices Applied

1. **Separation of Concerns**: Each class has single responsibility
2. **Dependency Injection**: Loose coupling, easy testing
3. **Async/Await**: Non-blocking operations
4. **MVVM Pattern**: Testable, maintainable code
5. **XML Documentation**: Comprehensive code comments
6. **Naming Conventions**: Clear, descriptive names
7. **Transactions**: Data consistency
8. **Navigation Properties**: Easy relationship access

---

## Testing Considerations

### Unit Testing ViewModel
```csharp
[Test]
public void IncrementCommand_IncrementsCounter()
{
    var mockService = new Mock<ICounterService>();
    var viewModel = new MainViewModel(mockService.Object);
    
    viewModel.IncrementCommand.Execute(null);
    
    mockService.Verify(s => s.IncrementCount(), Times.Once);
}
```

### Integration Testing Database
```csharp
[Test]
public async Task SaveReceiptWithItems_CreatesRelation()
{
    var dbService = new DatabaseService(dbPath);
    var receipt = new Receipt { StoreName = "Test" };
    var items = new List<ReceiptItem> { /* items */ };
    
    await dbService.SaveReceiptWithItemsAsync(receipt, items);
    
    var loaded = await dbService.GetReceiptWithItemsAsync(receipt.Id);
    Assert.AreEqual(loaded.Items.Count, items.Count);
}
```

---

## Performance Considerations

1. **Indexing**: ReceiptId is indexed for fast queries
2. **Async Operations**: Database calls don't block UI
3. **Lazy Loading**: Load related data only when needed
4. **Property Change Throttling**: Avoid excessive notifications

---

## Security Considerations

1. **SQL Injection**: SQLite-net-pcl uses parameterized queries
2. **Data Validation**: Validate input in ViewModels/Services
3. **Authentication**: Implement if needed
4. **Data Encryption**: Consider for sensitive data

---

## Future Enhancements

1. Add MVVM Toolkit for advanced patterns
2. Implement data encryption
3. Add cloud synchronization
4. Add unit test suite
5. Implement offline-first architecture
6. Add advanced search/filtering
7. Implement push notifications

---

**Last Updated**: 2025-01-13
**Version**: 1.0.0
