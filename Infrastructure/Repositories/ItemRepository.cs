using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;
using HouseInventoryTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseInventoryTracker.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _context.Items.OrderByDescending(i => i.UpdatedAt).ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await _context.Items.FindAsync(id);
    }

    public async Task<IEnumerable<Item>> SearchAsync(string? searchTerm, string? category, string? location)
    {
        var query = _context.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(i =>
                i.Name.ToLower().Contains(term) ||
                (i.Description != null && i.Description.ToLower().Contains(term)) ||
                (i.Category != null && i.Category.ToLower().Contains(term)) ||
                (i.Location != null && i.Location.ToLower().Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(i => i.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(i => i.Location == location);
        }

        return await query.OrderByDescending(i => i.UpdatedAt).ToListAsync();
    }

    public async Task AddAsync(Item item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        var existing = await _context.Items.FindAsync(item.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(item);
        }
        else
        {
            _context.Items.Update(item);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Items
            .Where(i => i.Category != null)
            .Select(i => i.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetLocationsAsync()
    {
        return await _context.Items
            .Where(i => i.Location != null)
            .Select(i => i.Location!)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();
    }
}
