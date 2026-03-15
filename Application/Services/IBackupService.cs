using System;
using System.IO;
using System.Threading.Tasks;

namespace HouseInventoryTracker.Application.Services;

public interface IBackupService
{
    string DatabasePath { get; }
    Task BackupAsync(string destinationPath);
    Task RestoreAsync(string sourcePath);
}
