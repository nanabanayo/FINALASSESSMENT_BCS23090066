using Microsoft.Maui.Devices.Sensors;
using MobileFinal.Services;

namespace MobileFinal;

public partial class TrackingPage : ContentPage
{
    DatabaseService _dbService = new DatabaseService();
    private bool _locationLoaded = false;

    public TrackingPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (!_locationLoaded)
        {
            await LoadData();
            _locationLoaded = true;
        }
    }

    private async Task LoadData()
    {
        try
        {
            // Request location permission at runtime (Android 6.0+)
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status != PermissionStatus.Granted)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Permission Denied", "Location permission is required to get GPS coordinates.", "OK");
                });
                return;
            }

            // 1. Get Connectivity 
            var access = Connectivity.Current.NetworkAccess;

            // 2. Get Location
            var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            // 3. Update UI on the Main Thread
            MainThread.BeginInvokeOnMainThread(() => {
                lblStatus.Text = access.ToString();
                if (location != null)
                {
                    lblLat.Text = location.Latitude.ToString("F6");
                    lblLong.Text = location.Longitude.ToString("F6");
                }
                else
                {
                    lblLat.Text = "No location";
                    lblLong.Text = "No location";
                }
            });
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await DisplayAlert("Error", "GPS not responding: " + ex.Message, "OK");
            });
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Synchronous Client-Side Validation 
        if (string.IsNullOrWhiteSpace(entryTripId.Text) || entryTripId.Text.Length < 5)
        {
            entryTripId.PlaceholderColor = Colors.Red;
            await DisplayAlert("Validation Error", "Trip ID must be at least 5 characters.", "OK");
            return;
        }

        await _dbService.SaveTripAsync(entryTripId.Text, lblLat.Text, lblLong.Text);
        await DisplayAlert("Success", "Data stored in SQLite database.", "OK");
    }
}