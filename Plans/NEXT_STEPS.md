# House Inventory Tracker - Remaining Steps

## Completed ✅
- Step 1: Feature Specification (SPEC.md)
- Step 2: Project Setup (Avalonia + MVVM + EF Core SQLite)
- Step 3: Main Window UI with DataGrid
- Step 4: Item Editor Dialog (Add/Edit items)
- Step 5: Data persistence (SQLite database)
- Step 6: Search and Filter functionality

## Next Steps (Priority Order)

### 1. CSV Export (P1 - High Priority)
**Files to modify/create:**
- Create `Application/Services/IExportService.cs`
- Create `Application/Services/ExportService.cs`
- Add export button to MainWindow toolbar
- Export all items or filtered items to CSV

**Implementation:**
```csharp
// Export format: Name,Quantity,Category,Location,Description,PurchasePrice,PurchaseDate
```

### 2. Delete Confirmation Dialog
**Files to modify:**
- `Views/MainWindowViewModel.cs` - Add confirmation dialog before delete

### 3. Photo Display in Details Panel
**Files to modify:**
- `Views/MainWindow.axaml` - Add Image control to show item photo

### 4. Backup/Restore Database (P1)
**Files to create:**
- Create backup functionality to copy SQLite database file
- Create restore functionality to replace database file

### 5. Print Functionality (P1)
**Files to modify:**
- Add print button to print inventory list

### 6. Future: Android Sync API (P2)
**Already prepared:**
- GUID-based item IDs
- CreatedAt/UpdatedAt timestamps
- JSON serialization ready

**Future implementation would include:**
- Web API endpoints for sync
- Android app with matching data model

---

## How to Continue

1. Navigate to: `/home/neo/Projects/HouseInventoryTracker`
2. Run: `dotnet run` to test
3. Implement next feature from the list above

## Running the App
```bash
cd /home/neo/Projects/HouseInventoryTracker
dotnet run
```

## Current Features
- Add/Edit/Delete inventory items
- Search by name, category, location
- DataGrid with sorting
- SQLite persistence in `~/.local/share/HouseInventoryTracker/`
- Logs in `~/.local/share/HouseInventoryTracker/logs/`

## Build for Production
```bash
cd /home/neo/Projects/HouseInventoryTracker
dotnet publish -c Release -r linux-x64 --self-contained
```
