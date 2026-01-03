using SQLite;

namespace MobileFinal.Services;

public class TripRecord
{
    [PrimaryKey, AutoIncrement] // Uncomment these!
    public int ID { get; set; }
    public string TripId { get; set; }
    public string GeolocationData { get; set; }
}

public class DatabaseService
{
    SQLiteAsyncConnection _db;
    async Task Init()
    {
        if (_db != null) return;
        var path = Path.Combine(FileSystem.AppDataDirectory, "VehicleLog.db3");
        _db = new SQLiteAsyncConnection(path);
        await _db.CreateTableAsync<TripRecord>(); // Step 4: Core Integration 
    }

    public async Task SaveTripAsync(string tripId, string lat, string lng)
    {
        await Init();
        await _db.InsertAsync(new TripRecord { TripId = tripId, GeolocationData = $"{lat},{lng}" });
    }
}