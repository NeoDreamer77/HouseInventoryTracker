using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseInventoryTracker.Application.Services;
using HouseInventoryTracker.Domain.Entities;
using Serilog;

namespace HouseInventoryTracker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IItemService _itemService;
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

    public MainWindowViewModel(IItemService itemService)
    {
        _itemService = itemService;
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
        Items = new ObservableCollection<Item>(items);
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
        var dialog = new Views.ItemEditorDialog
        {
            DataContext = this,
            EditingItem = newItem
        };

        await dialog.ShowDialog(_mainWindow);

        if (!string.IsNullOrWhiteSpace(newItem.Name))
        {
            await _itemService.AddItemAsync(newItem);
            await LoadItemsAsync();
            await LoadFiltersAsync();
            await UpdateSummaryAsync();
        }
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
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null) return;
        
        await _itemService.DeleteItemAsync(SelectedItem.Id);
        await LoadItemsAsync();
        await LoadFiltersAsync();
        await UpdateSummaryAsync();
        SelectedItem = null;
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
