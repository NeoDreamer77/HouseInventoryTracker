using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HouseInventoryTracker.Domain.Entities;

public class Item : INotifyPropertyChanged
{
    private Guid _id = Guid.NewGuid();
    private string _name = string.Empty;
    private int _quantity = 1;
    private string? _description;
    private string? _category;
    private string? _location;
    private string? _photoPath;
    private DateTime? _purchaseDate;
    private decimal? _purchasePrice;
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime _updatedAt = DateTime.UtcNow;

    public Guid Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetField(ref _quantity, value);
    }

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public string? Category
    {
        get => _category;
        set => SetField(ref _category, value);
    }

    public string? Location
    {
        get => _location;
        set => SetField(ref _location, value);
    }

    public string? PhotoPath
    {
        get => _photoPath;
        set => SetField(ref _photoPath, value);
    }

    public DateTime? PurchaseDate
    {
        get => _purchaseDate;
        set => SetField(ref _purchaseDate, value);
    }

    public decimal? PurchasePrice
    {
        get => _purchasePrice;
        set => SetField(ref _purchasePrice, value);
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetField(ref _createdAt, value);
    }

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set => SetField(ref _updatedAt, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
