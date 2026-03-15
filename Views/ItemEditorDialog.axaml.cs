using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HouseInventoryTracker.Domain.Entities;
using HouseInventoryTracker.ViewModels;

namespace HouseInventoryTracker.Views;

public partial class ItemEditorDialog : Window
{
    public static readonly StyledProperty<Item?> EditingItemProperty =
        AvaloniaProperty.Register<ItemEditorDialog, Item?>(nameof(EditingItem));

    public Item? EditingItem
    {
        get => GetValue(EditingItemProperty);
        set => SetValue(EditingItemProperty, value);
    }

    public ItemEditorDialog()
    {
        InitializeComponent();
    }

    private async void BrowsePhotoClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && vm.EditingItem != null)
        {
            var topLevel = GetTopLevel(this);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Photo",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp" }
                    }
                }
            });

            if (files.Count > 0)
            {
                vm.EditingItem.PhotoPath = files[0].Path.LocalPath;
            }
        }
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.EditingItem = null;
        }
        Close();
    }

    private void SaveClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
