# House Inventory Tracker - Remaining Steps

## Completed ✅
- Step 1: Feature Specification (SPEC.md)
- Step 2: Project Setup (Avalonia + MVVM + EF Core SQLite)
- Step 3: Main Window UI with DataGrid
- Step 4: Item Editor Dialog (Add/Edit items)
- Step 5: Data persistence (SQLite database)
- Step 6: Search and Filter functionality
- **P1: CSV Export** - File → Export to CSV
- **P1: Delete Confirmation Dialog** - Confirmation before delete
- **P1: Photo Display in Details Panel** - Shows photo when item selected
- **P1: Backup/Restore Database** - File → Backup/Restore
- **P1: Print Functionality** - File → Print Preview (basic)

## Remaining Tasks

### Print Functionality (Enhancement Needed)
- Native print dialog not available in current Avalonia version
- Current workaround: Preview → Ctrl+P → Save as PDF or print
- Future: Implement native printing when Avalonia adds support

### Future Features (P2)
- Android Sync API
  - GUID-based item IDs (done)
  - CreatedAt/UpdatedAt timestamps (done)
  - JSON serialization ready

---

## Running the App
```bash
cd /home/neo/Projects/HouseInventoryTracker
dotnet run
```

## Current Features
- Add/Edit/Delete inventory items
- Search by name, category, location
- DataGrid with sorting
- Photo support (browse, display in details)
- CSV Export
- Backup/Restore with WAL file support
- Print Preview
- SQLite persistence in `~/.local/share/HouseInventoryTracker/`
- Logs in `~/.local/share/HouseInventoryTracker/logs/`

## Build for Production
```bash
cd /home/neo/Projects/HouseInventoryTracker
dotnet publish -c Release -r linux-x64 --self-contained
```
