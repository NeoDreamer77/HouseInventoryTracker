# HouseInventoryTracker Errors and Resolutions

## Error 1: Missing System usings in multiple files

**Error:** `CS0246: The type or namespace name 'Task<>' could not be found`

**Cause:** Files were missing `using System;`, `using System.Collections.Generic;`, `using System.Linq;`, `using System.Threading.Tasks;`

**Resolution:** Added the missing using statements to:
- `/home/neo/Projects/HouseInventoryTracker/Application/Services/IItemService.cs`
- `/home/neo/Projects/HouseInventoryTracker/Application/Services/ItemService.cs`
- `/home/neo/Projects/HouseInventoryTracker/Infrastructure/Repositories/IItemRepository.cs`
- `/home/neo/Projects/HouseInventoryTracker/Infrastructure/Repositories/ItemRepository.cs`
- `/home/neo/Projects/HouseInventoryTracker/Domain/Entities/Item.cs`
- `/home/neo/Projects/HouseInventoryTracker/Program.cs`

---

## Error 2: Namespace conflict with Application

**Error:** `CS0118: 'Application' is a namespace but is used like a type`

**Cause:** The folder name `Application` conflicted with `Avalonia.Application`

**Resolution:** Changed `App : Application` to `App : Avalonia.Application` in `App.axaml.cs`

---

## Error 3: MVVM Toolkit RelayCommand incompatibility

**Error:** `MVVMTK0007: The method ... cannot be used to generate a command property, as its signature isn't compatible with any of the existing relay command types`

**Cause:** Methods like `Search()`, `ClearFilters()`, `DeleteItem()`, and `SaveItem()` returned `Task` but were marked with `[RelayCommand]`

**Resolution:** 
- Changed method names to have `Async` suffix (e.g., `SearchAsync`, `DeleteItemAsync`, `SaveItemAsync`)
- Methods that don't need async behavior keep void return (e.g., `AddItem`, `EditItem`, `CancelEdit`)

---

## Error 4: Missing System.IO in Program.cs

**Error:** `CS0103: The name 'Path' does not exist in the current context`

**Resolution:** Added `using System.IO;` to Program.cs

---

## Error 5: ItemEditorDialog missing EditingItem property

**Error:** `CS0117: 'ItemEditorDialog' does not contain a definition for 'EditingItem'`

**Resolution:** Added `EditingItem` as a StyledProperty to `ItemEditorDialog.axaml.cs`

---

## Error 6: Missing Avalonia using in ItemEditorDialog

**Error:** `CS0246: The type or namespace name 'StyledProperty<>' could not be found`

**Resolution:** Added `using Avalonia;` and changed to `AvaloniaProperty.Register<>()`

---

## Error 7: Missing DataGrid package

**Error:** `System.MethodAccessException: Attempt by method 'HouseInventoryTracker.Views.MainWindow.!XamlIlPopulate' to access method 'Avalonia.Controls.DataGrid..ctor()' failed.`

**Cause:** DataGrid is in a separate NuGet package

**Resolution:** 
1. Added `Avalonia.Controls.DataGrid` package to csproj
2. Added DataGrid styles to App.axaml: `<StyleInclude Source="avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"/>`
