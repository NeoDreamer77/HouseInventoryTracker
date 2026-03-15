using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;
using HouseInventoryTracker.Infrastructure.Repositories;

namespace HouseInventoryTracker.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;

    public ItemService(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Item?> GetItemByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Item>> SearchItemsAsync(string? searchTerm, string? category, string? location)
    {
        return await _repository.SearchAsync(searchTerm, category, location);
    }

    public async Task AddItemAsync(Item item)
    {
        await _repository.AddAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        await _repository.UpdateAsync(item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetAllCategoriesAsync()
    {
        return await _repository.GetCategoriesAsync();
    }

    public async Task<IEnumerable<string>> GetAllLocationsAsync()
    {
        return await _repository.GetLocationsAsync();
    }

    public async Task<decimal> GetTotalValueAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Sum(i => i.PurchasePrice ?? 0 * i.Quantity);
    }

    public async Task<int> GetTotalItemCountAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Sum(i => i.Quantity);
    }
}
