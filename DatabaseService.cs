using SQLite;

namespace MobileFinal.Services;

public class TripRecord
{
    [PrimaryKey, AutoIncrement]
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
        await _db.CreateTableAsync<TripRecord>();
    }

    public async Task SaveTripAsync(string tripId, string lat, string lng)
    {
        await Init();
        await _db.InsertAsync(new TripRecord { TripId = tripId, GeolocationData = $"{lat},{lng}" });
    }
    public async Task<List<TripRecord>> GetTripsAsync()
    {
        await Init();
        return await _db.Table<TripRecord>().ToListAsync();
    }
}
