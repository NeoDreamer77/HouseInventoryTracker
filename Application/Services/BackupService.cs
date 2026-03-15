using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HouseInventoryTracker.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Serilog;

namespace HouseInventoryTracker.Application.Services;

public class BackupService : IBackupService
{
    private readonly string _dbPath;

    public BackupService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "HouseInventoryTracker");
        Directory.CreateDirectory(appFolder);
        _dbPath = Path.Combine(appFolder, "inventory.db");
    }

    public string DatabasePath => _dbPath;

    public async Task BackupAsync(string destinationPath)
    {
        if (!File.Exists(_dbPath))
        {
            throw new FileNotFoundException("Database file not found", _dbPath);
        }

        var destDir = Path.GetDirectoryName(destinationPath);
        var destName = Path.GetFileNameWithoutExtension(destinationPath);
        
        if (!string.IsNullOrEmpty(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        await Task.Run(() =>
        {
            // Copy main database file
            File.Copy(_dbPath, destinationPath, overwrite: true);
            
            // Also copy WAL file if it exists
            var sourceWal = _dbPath + "-wal";
            var destWal = Path.Combine(destDir!, destName + ".db-wal");
            if (File.Exists(sourceWal))
            {
                File.Copy(sourceWal, destWal, overwrite: true);
            }
            
            // Also copy SHM file if it exists
            var sourceShm = _dbPath + "-shm";
            var destShm = Path.Combine(destDir!, destName + ".db-shm");
            if (File.Exists(sourceShm))
            {
                File.Copy(sourceShm, destShm, overwrite: true);
            }
        });
        
        Log.Information("Database backed up to {Path}", destinationPath);
    }

    public async Task RestoreAsync(string sourcePath)
    {
        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException("Backup file not found", sourcePath);
        }

        var sourceDir = Path.GetDirectoryName(sourcePath);
        var sourceName = Path.GetFileNameWithoutExtension(sourcePath);
        
        await Task.Run(() =>
        {
            // Copy the backup file
            File.Copy(sourcePath, _dbPath, overwrite: true);

            // Also copy WAL file from backup if it exists
            var sourceWal = sourceName + ".db-wal";
            var sourceWalPath = Path.Combine(sourceDir!, sourceWal);
            var destWal = _dbPath + "-wal";
            if (File.Exists(sourceWalPath) && new FileInfo(sourceWalPath).Length > 0)
            {
                File.Copy(sourceWalPath, destWal, overwrite: true);
            }

            // Also copy SHM file from backup if it exists
            var sourceShm = sourceName + ".db-shm";
            var sourceShmPath = Path.Combine(sourceDir!, sourceShm);
            var destShm = _dbPath + "-shm";
            if (File.Exists(sourceShmPath))
            {
                File.Copy(sourceShmPath, destShm, overwrite: true);
            }
            
            // Delete local empty WAL files
            var localWal = _dbPath + "-wal";
            var localShm = _dbPath + "-shm";
            if (File.Exists(localWal) && new FileInfo(localWal).Length == 0)
            {
                File.Delete(localWal);
            }
            if (File.Exists(localShm))
            {
                File.Delete(localShm);
            }
        });

        Log.Information("Database restored from {Path}", sourcePath);
    }
}
