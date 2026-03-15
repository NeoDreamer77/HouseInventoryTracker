using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;

namespace HouseInventoryTracker.Application.Services;

public interface IExportService
{
    Task ExportToCsvAsync(IEnumerable<Item> items, string filePath, CancellationToken cancellationToken = default);
}
