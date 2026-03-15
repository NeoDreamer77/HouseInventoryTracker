# House Inventory Tracker

A cross-platform desktop application for tracking household items with photo support, search/filter capabilities, and future Android sync capability.

## Features

- **Item Management** - Add, edit, and delete inventory items with details like name, quantity, category, location, description, photo, purchase date, and price
- **Search & Filter** - Real-time search across all item fields, filter by category and location
- **DataGrid Display** - Sortable columns with item thumbnails
- **SQLite Persistence** - Local database stored in user's app data directory
- **Logging** - Structured logging for debugging and monitoring

## Technology Stack

- **Framework:** Avalonia 12 (cross-platform UI)
- **Language:** C# (.NET 10)
- **Database:** SQLite via EF Core
- **Architecture:** MVVM pattern

## Getting Started

### Prerequisites

- .NET 10 SDK
- Avalonia templates

### Build & Run

```bash
cd HouseInventoryTracker
dotnet run
```

### Build for Production

```bash
dotnet publish -c Release -r linux-x64 --self-contained
```

## Data Storage

- **Database:** `~/.local/share/HouseInventoryTracker/inventory.db`
- **Logs:** `~/.local/share/HouseInventoryTracker/logs/`

## Project Structure

```
HouseInventoryTracker/
├── Application/Services/       # Business logic
├── Domain/Entities/            # Domain models
├── Infrastructure/             # Data access, repositories
├── ViewModels/                 # MVVM ViewModels
├── Views/                      # Avalonia views
└── Assets/                     # Images, icons
```

## License

MIT
