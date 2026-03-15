# House Inventory Tracker - Feature Specification

## Project Overview

**Project Name:** HouseInventoryTracker  
**Type:** Desktop Application (Avalonia UI / .NET)  
**Core Feature Summary:** A cross-platform desktop application for tracking household items with photo support, search/filter capabilities, and future Android sync capability.  
**Target Users:** Homeowners, renters, and anyone wanting to catalog their belongings for insurance, organization, or moving purposes.

---

## Core Features (P0 - Must Have)

### F1: Item Management
- **F1.1** Add new inventory items with:
  - Name (required, string, max 200 chars)
  - Quantity (required, integer, min 1)
  - Description (optional, string, max 2000 chars)
  - Category (optional, string, max 100 chars)
  - Location/Room (optional, string, max 100 chars)
  - Photo (optional, file path to image)
  - Purchase Date (optional, date)
  - Purchase Price (optional, decimal)
- **F1.2** Edit existing items (all fields)
- **F1.3** Delete items with confirmation dialog
- **F1.4** View item details in a detail panel or dialog

### F2: Inventory Display
- **F2.1** Display all items in a sortable DataGrid
- **F2.2** Sort by any column (Name, Quantity, Category, Location, Date Added)
- **F2.3** Show item photo thumbnails in the grid
- **F2.4** Display item count summary (total items, total value)

### F3: Search & Filter
- **F3.1** Real-time text search across Name, Description, Category, Location
- **F3.2** Filter by Category (dropdown)
- **F3.3** Filter by Location/Room (dropdown)
- **F3.4** Clear all filters button

### F4: Data Persistence
- **F4.1** SQLite database for local storage
- **F4.2** Database stored in user's app data directory
- **F4.3** Auto-create database and tables on first launch
- **F4.4** Data survives app restarts

---

## Secondary Features (P1 - Should Have)

### F5: Reports
- **F5.1** Generate inventory summary report
- **F5.2** Export to CSV
- **F5.3** Print capability

### F6: Data Management
- **F6.1** Backup database to user-selected location
- **F6.2** Restore database from backup file

---

## Future Features (P2 - Nice to Have)

### F7: Android Sync (Future)
- **F7.1** API-ready data structure (JSON serialization support)
- **F7.2** Unique item IDs (GUIDs) for sync conflict resolution
- **F7.3** Export/Import JSON format for manual sync
- **F7.4** Timestamps (CreatedAt, UpdatedAt) for sync tracking

---

## Technical Specifications

### Technology Stack
- **Framework:** Avalonia 12 (cross-platform UI)
- **Language:** C# (.NET 8)
- **Database:** SQLite via Microsoft.Data.Sqlite or EF Core SQLite
- **Architecture:** MVVM pattern
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection

### Database Schema

```sql
CREATE TABLE Items (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Quantity INTEGER NOT NULL DEFAULT 1,
    Description TEXT,
    Category TEXT,
    Location TEXT,
    PhotoPath TEXT,
    PurchaseDate TEXT,
    PurchasePrice REAL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL
);

CREATE INDEX IX_Items_Category ON Items(Category);
CREATE INDEX IX_Items_Location ON Items(Location);
CREATE INDEX IX_Items_Name ON Items(Name);
```

### Project Structure (Vertical Slice)
```
src/
  HouseInventoryTracker/
    Domain/
      Entities/
        Item.cs
      ValueObjects/
    Application/
      Services/
        IItemService.cs
        ItemService.cs
      DTOs/
    Infrastructure/
      Data/
        AppDbContext.cs
      Repositories/
        IItemRepository.cs
        ItemRepository.cs
    Presentation/
      ViewModels/
        MainWindowViewModel.cs
        ItemEditorViewModel.cs
      Views/
        MainWindow.axaml
        ItemEditorDialog.axaml
      Controls/
      Converters/
    App.axaml
    Program.cs
tests/
  HouseInventoryTracker.Tests/
```

---

## Non-Functional Requirements

### Performance
- App launches in under 3 seconds
- Search/filter responds in under 100ms for up to 10,000 items
- Photo thumbnails load progressively

### Security
- No sensitive data stored in plain text
- File paths sanitized to prevent path traversal

### Usability
- Keyboard navigation support
- Consistent visual feedback for all actions
- Error messages are user-friendly

---

## Out of Scope (v1.0)
- User authentication
- Multi-device sync (future)
- Cloud storage
- Barcode/QR scanning
- Voice input
