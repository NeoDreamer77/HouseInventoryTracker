using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;
using HouseInventoryTracker.Infrastructure.Repositories;

namespace HouseInventoryTracker.Application.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(Guid id);
    Task<IEnumerable<Item>> SearchItemsAsync(string? searchTerm, string? category, string? location);
    Task AddItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task DeleteItemAsync(Guid id);
    Task<IEnumerable<string>> GetAllCategoriesAsync();
    Task<IEnumerable<string>> GetAllLocationsAsync();
    Task<decimal> GetTotalValueAsync();
    Task<int> GetTotalItemCountAsync();
}
