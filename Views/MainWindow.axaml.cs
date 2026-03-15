using Avalonia.Controls;
using HouseInventoryTracker.ViewModels;

namespace HouseInventoryTracker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.SetMainWindow(this);
        }
    }
}
