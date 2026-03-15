using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HouseInventoryTracker.Domain.Entities;
using Serilog;

namespace HouseInventoryTracker.Application.Services;

public class ExportService : IExportService
{
    public async Task ExportToCsvAsync(IEnumerable<Item> items, string filePath, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Name,Quantity,Category,Location,Description,PurchasePrice,PurchaseDate");

        foreach (var item in items)
        {
            var name = EscapeCsvField(item.Name);
            var quantity = item.Quantity.ToString();
            var category = EscapeCsvField(item.Category ?? "");
            var location = EscapeCsvField(item.Location ?? "");
            var description = EscapeCsvField(item.Description ?? "");
            var price = item.PurchasePrice?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
            var date = item.PurchaseDate?.ToString("yyyy-MM-dd") ?? "";

            sb.AppendLine($"{name},{quantity},{category},{location},{description},{price},{date}");
        }

        await File.WriteAllTextAsync(filePath, sb.ToString(), cancellationToken);
        Log.Information("Exported {Count} items to {FilePath}", items is ICollection<Item> c ? c.Count : 0, filePath);
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}
