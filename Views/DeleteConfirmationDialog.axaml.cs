using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HouseInventoryTracker.Views;

public partial class DeleteConfirmationDialog : Window
{
    public bool Confirmed { get; private set; }

    public DeleteConfirmationDialog()
    {
        InitializeComponent();
    }

    public DeleteConfirmationDialog(string itemName) : this()
    {
        ItemNameText.Text = itemName;
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        Confirmed = false;
        Close();
    }

    private void DeleteClick(object? sender, RoutedEventArgs e)
    {
        Confirmed = true;
        Close();
    }
}
