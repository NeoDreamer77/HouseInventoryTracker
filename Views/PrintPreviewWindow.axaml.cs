using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HouseInventoryTracker.Domain.Entities;

namespace HouseInventoryTracker.Views;

public partial class PrintPreviewWindow : Window
{
    private readonly IEnumerable<Item> _items;
    private readonly int _totalItems;
    private readonly decimal _totalValue;

    public PrintPreviewWindow(IEnumerable<Item> items, int totalItems, decimal totalValue, string? categoryFilter, string? locationFilter, string? searchFilter)
    {
        InitializeComponent();

        _items = items;
        _totalItems = totalItems;
        _totalValue = totalValue;

        DateText.Text = $"Printed: {DateTime.Now:MMMM dd, yyyy h:mm tt}";
        TotalItemsText.Text = $"Total Items: {_totalItems}";
        TotalValueText.Text = $"Total Value: {_totalValue:C}";

        var filters = new List<string>();
        if (!string.IsNullOrWhiteSpace(searchFilter))
            filters.Add($"Search: {searchFilter}");
        if (!string.IsNullOrWhiteSpace(categoryFilter))
            filters.Add($"Category: {categoryFilter}");
        if (!string.IsNullOrWhiteSpace(locationFilter))
            filters.Add($"Location: {locationFilter}");
        FilterText.Text = filters.Count > 0 ? string.Join(", ", filters) : "None";

        ItemsList.ItemsSource = items;
    }

    private async void PrintClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new Window
        {
            Title = "Print",
            Width = 350,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel 
            { 
                Margin = new Avalonia.Thickness(20), 
                Spacing = 15, 
                Children = {
                    new TextBlock { Text = "To print this report:", FontWeight = Avalonia.Media.FontWeight.Bold },
                    new TextBlock { Text = "1. Close this dialog", TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new TextBlock { Text = "2. Press Ctrl+P (or Cmd+P on Mac)", TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new TextBlock { Text = "3. Choose 'Save as PDF' or select your printer", TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new Button { Content = "OK", Width = 100, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center }
                }
            }
        };
        var btn = ((StackPanel)dialog.Content).Children.OfType<Button>().First();
        btn.Click += (s, args) => dialog.Close();
        await dialog.ShowDialog(this);
    }

    private void CloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
