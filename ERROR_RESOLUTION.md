# HouseInventoryTracker - Error Log

## Restore Not Working - Issue Resolution

### Problem
Restore from backup button was not working - clicking restore would complete without error but the app still showed 0 items.

### Attempts That Did NOT Work
1. Simple File.Copy - file copies correctly but app shows 0 items
2. Delete WAL/SHM files after copying backup - still shows 0 items
3. Delete source WAL/SHM files before copying - still shows 0 items
4. Force journal_mode=DELETE pragma - still shows 0 items
5. Create fresh DbContext after restore - still shows 0 items
6. Force close connection before restore - still shows 0 items

### Root Cause
The app was running with an open connection to an empty database (created via EnsureCreated). Even after copying the backup file, EF Core was caching the empty database state. Additionally, when the app created a backup, it used WAL (Write-Ahead Logging) mode, which stores the actual data in the .db-wal file, not just in the main .db file.

### Solution That Worked
1. Remove `EnsureCreated()` at startup - only connect if database already exists
2. When restoring, copy BOTH the main database file AND the WAL/SHM files from the backup

### Code Fix
```csharp
// Backup - copy WAL files
var sourceWal = _dbPath + "-wal";
var destWal = Path.Combine(destDir, destName + ".db-wal");
if (File.Exists(sourceWal))
{
    File.Copy(sourceWal, destWal, overwrite: true);
}

// Restore - copy WAL files from backup
var sourceWalPath = Path.Combine(sourceDir, sourceName + ".db-wal");
if (File.Exists(sourceWalPath) && new FileInfo(sourceWalPath).Length > 0)
{
    File.Copy(sourceWalPath, destWal, overwrite: true);
}
```

### Key Learning
SQLite WAL mode stores data in separate files. When backing up or restoring SQLite databases in WAL mode, you must copy ALL files (.db, .db-wal, .db-shm) together.
