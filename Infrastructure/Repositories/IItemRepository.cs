using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;

namespace HouseInventoryTracker.Infrastructure.Repositories;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(Guid id);
    Task<IEnumerable<Item>> SearchAsync(string? searchTerm, string? category, string? location);
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task<IEnumerable<string>> GetLocationsAsync();
}
