using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseInventoryTracker.Application.Services;
using HouseInventoryTracker.Domain.Entities;
using Serilog;

namespace HouseInventoryTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IItemService _itemService;
    private readonly IExportService _exportService;
    private readonly IBackupService _backupService;
    private Window? _mainWindow;
    
    [ObservableProperty]
    private ObservableCollection<Item> _items = new();
    
    [ObservableProperty]
    private Item? _selectedItem;
    
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private string? _selectedCategory;
    
    [ObservableProperty]
    private string? _selectedLocation;
    
    [ObservableProperty]
    private ObservableCollection<string> _categories = new();
    
    [ObservableProperty]
    private ObservableCollection<string> _locations = new();
    
    [ObservableProperty]
    private int _totalItemCount;
    
    [ObservableProperty]
    private decimal _totalValue;
    
    [ObservableProperty]
    private Item? _editingItem;

    public MainWindowViewModel(IItemService itemService, IExportService exportService, IBackupService backupService)
    {
        _itemService = itemService;
        _exportService = exportService;
        _backupService = backupService;
    }

    public void SetMainWindow(Window window)
    {
        _mainWindow = window;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadItemsAsync();
            await LoadFiltersAsync();
            await UpdateSummaryAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize MainWindowViewModel");
        }
    }

    private async Task LoadItemsAsync()
    {
        var items = await _itemService.SearchItemsAsync(SearchText, SelectedCategory, SelectedLocation);
        var itemsList = items.ToList();
        Items = new ObservableCollection<Item>(itemsList);
    }

    private async Task LoadFiltersAsync()
    {
        var categories = await _itemService.GetAllCategoriesAsync();
        Categories = new ObservableCollection<string>(categories);
        
        var locations = await _itemService.GetAllLocationsAsync();
        Locations = new ObservableCollection<string>(locations);
    }

    private async Task UpdateSummaryAsync()
    {
        TotalItemCount = await _itemService.GetTotalItemCountAsync();
        TotalValue = await _itemService.GetTotalValueAsync();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadItemsAsync();
    }

    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        SearchText = string.Empty;
        SelectedCategory = null;
        SelectedLocation = null;
        await LoadItemsAsync();
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        if (_mainWindow == null) return;

        var newItem = new Item();
        EditingItem = newItem;  // Set ViewModel's EditingItem so bindings work
        
        var dialog = new Views.ItemEditorDialog
        {
            DataContext = this,
            EditingItem = newItem
        };

        await dialog.ShowDialog(_mainWindow);

        Log.Information("AddItemAsync: After dialog, Name={Name}, Quantity={Quantity}", newItem.Name, newItem.Quantity);
        
        if (!string.IsNullOrWhiteSpace(newItem.Name))
        {
            Log.Information("AddItemAsync: Saving item to database");
            await _itemService.AddItemAsync(newItem);
            await LoadItemsAsync();
            await LoadFiltersAsync();
            await UpdateSummaryAsync();
            Log.Information("AddItemAsync: Item saved successfully");
        }
        else
        {
            Log.Warning("AddItemAsync: Item name is empty, not saving");
        }
        
        EditingItem = null;  // Clear after done
    }

    [RelayCommand]
    private async Task EditItemAsync()
    {
        if (SelectedItem == null || _mainWindow == null) return;
        
        var editItem = new Item
        {
            Id = SelectedItem.Id,
            Name = SelectedItem.Name,
            Quantity = SelectedItem.Quantity,
            Description = SelectedItem.Description,
            Category = SelectedItem.Category,
            Location = SelectedItem.Location,
            PhotoPath = SelectedItem.PhotoPath,
            PurchaseDate = SelectedItem.PurchaseDate,
            PurchasePrice = SelectedItem.PurchasePrice,
            CreatedAt = SelectedItem.CreatedAt,
            UpdatedAt = SelectedItem.UpdatedAt
        };

        EditingItem = editItem;  // Set ViewModel's EditingItem so bindings work
        
        var dialog = new Views.ItemEditorDialog
        {
            DataContext = this,
            EditingItem = editItem
        };

        await dialog.ShowDialog(_mainWindow);

        if (!string.IsNullOrWhiteSpace(editItem.Name))
        {
            await _itemService.UpdateItemAsync(editItem);
            await LoadItemsAsync();
            await LoadFiltersAsync();
            await UpdateSummaryAsync();
        }
        
        EditingItem = null;  // Clear after done
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null || _mainWindow == null) return;

        var dialog = new Views.DeleteConfirmationDialog(SelectedItem.Name);
        await dialog.ShowDialog(_mainWindow);

        if (!dialog.Confirmed) return;

        await _itemService.DeleteItemAsync(SelectedItem.Id);
        await LoadItemsAsync();
        await LoadFiltersAsync();
        await UpdateSummaryAsync();
        SelectedItem = null;
    }

    [RelayCommand]
    private async Task ExportAsync()
    {
        if (_mainWindow == null || Items.Count == 0) return;

        var topLevel = _mainWindow;
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export Inventory to CSV",
            SuggestedFileName = $"inventory_export_{DateTime.Now:yyyyMMdd}",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("CSV Files")
                {
                    Patterns = new[] { "*.csv" }
                }
            }
        });

        if (file != null)
        {
            await _exportService.ExportToCsvAsync(Items, file.Path.LocalPath);
            Log.Information("Exported {Count} items to CSV", Items.Count);
        }
    }

    [RelayCommand]
    private async Task BackupAsync()
    {
        if (_mainWindow == null) return;

        var file = await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Backup Database",
            SuggestedFileName = $"inventory_backup_{DateTime.Now:yyyyMMdd}",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("SQLite Database")
                {
                    Patterns = new[] { "*.db" }
                }
            }
        });

        if (file != null)
        {
            await _backupService.BackupAsync(file.Path.LocalPath);
            Log.Information("Database backed up to {Path}", file.Path.LocalPath);
        }
    }

    [RelayCommand]
    private async Task RestoreAsync()
    {
        if (_mainWindow == null) return;

        var file = await _mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Restore Database",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("SQLite Database")
                {
                    Patterns = new[] { "*.db" }
                }
            }
        });

        if (file.Count > 0)
        {
            await _backupService.RestoreAsync(file[0].Path.LocalPath);
            await LoadItemsAsync();
            await LoadFiltersAsync();
            await UpdateSummaryAsync();
            Log.Information("Database restored from {Path}", file[0].Path.LocalPath);
        }
    }

    [RelayCommand]
    private void CancelEdit()
    {
        // Dialog handles closing itself
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = LoadItemsAsync();
    }

    partial void OnSelectedCategoryChanged(string? value)
    {
        _ = LoadItemsAsync();
    }

    partial void OnSelectedLocationChanged(string? value)
    {
        _ = LoadItemsAsync();
    }
}
