using Microsoft.Maui.Devices.Sensors;

namespace MobileFinal;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent(); // This connects the XAML UI to this code
    }

    private async void OnStartTripClicked(object sender, EventArgs e)
    {
        // This moves the user from the Start screen to the Tracking screen
        await Navigation.PushAsync(new TrackingPage());
    }
}