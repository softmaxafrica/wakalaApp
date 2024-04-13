namespace mobilewakala.Agent;

 using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobilewakala.Models;
using mobilewakala.Services;
using Map = Microsoft.Maui.Controls.Maps.Map;

public partial class AgentCustMap : ContentPage
{
    public OpenTicketModel _mapData;
    private Map map;
    public IAgentServices _AgentServices;

    public AgentCustMap(OpenTicketModel mapdata)
    {
        InitializeComponent();
        _AgentServices = new AgentServices();
        _mapData = mapdata;
        map = new Map();
        Content = map;

        map.MapType = MapType.Street;
        map.IsTrafficEnabled = true;
        map.IsShowingUser = true;

        AttendTicket();
    }

    private async Task AttendTicket()
    {
        bool result = await _AgentServices.AttendCustTicket(_mapData);
        if (result)
        {
            ShowCustomerLocationOnMap();

        }
    }
    private async void ShowCustomerLocationOnMap()
    {



        // Center the map on the customer's location
        Location location = new Location(_mapData.customerLatitude, _mapData.customerLongitude);
        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1));
        map.MoveToRegion(mapSpan);

        // Add a pin for the customer's location
        var pin = new Pin
        {
            Label = "Mteja",
            Location = location,
            Type = PinType.Place,
            Address = _mapData.phoneNumber
        };
        map.Pins.Add(pin);
        await GetDirections_Clicked();
        //await DrawDirections();
    }

    private async Task GetDirections_Clicked()
    {
        try
        {
            // Get the current user's location
            var currentLocation = await Geolocation.GetLocationAsync();

            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Walking };

            string url = $"http://maps.google.com/maps?&daddr={_mapData.customerLatitude},{_mapData.customerLongitude}&saddr={currentLocation.Latitude},{currentLocation.Longitude}&dirflg=d";
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
            https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                await Launcher.OpenAsync("http://maps.apple.com/?daddr={_mapData.customerLatitude},{_mapData.customerLongitude}&saddr={currentLocation.Latitude},{currentLocation.Longitude");
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                await Launcher.OpenAsync(url);
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                await Launcher.OpenAsync(url);
            }

            //await DrawDirections();

        }
        catch (Exception ex)
        {
            // Handle exceptions
            await DisplayAlert("Error", $"Unable to get directions: {ex.Message}", "OK");
        }
    }


    //#region Handling Rout Directions
    //private async Task DrawDirections()
    //{
    //    try
    //    {
    //        // Get the current user's location
    //        var currentLocation = await Geolocation.GetLocationAsync();

    //        // Fetch directions using Google Maps Directions API
    //        var httpClient = new HttpClient();
    //        var response = await httpClient.GetAsync($"https://maps.googleapis.com/maps/api/directions/json?origin={currentLocation}," +
    //                        $"{currentLocation.Longitude}&destination={_mapData.customerLatitude},{_mapData.customerLongitude}&key=AIzaSyBtyb_hRlQoEhe1aJMEbKL02JvFTIC6kI4");

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var jsonString = await response.Content.ReadAsStringAsync();
    //            var route = ParseRoute(jsonString);
    //            DrawRoute(route);
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "Failed to fetch directions", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Unable to fetch directions: {ex.Message}", "OK");
    //    }
    //}
    //private List<(double latitude, double longitude)> ParseRoute(string jsonString)
    //{
    //    var routePoints = new List<(double latitude, double longitude)>();

    //    var json = JObject.Parse(jsonString);
    //    var routes = json["routes"];
    //    if (routes != null && routes.HasValues)
    //    {
    //        var legs = routes[0]["legs"];
    //        if (legs != null && legs.HasValues)
    //        {
    //            var steps = legs[0]["steps"];
    //            if (steps != null && steps.HasValues)
    //            {
    //                foreach (var step in steps)
    //                {
    //                    var polylinePoints = step["polyline"]["points"].ToString();
    //                    routePoints.AddRange(DecodePolyline(polylinePoints));
    //                }
    //            }
    //        }
    //    }

    //    return routePoints;
    //}

    //private void DrawRoute(List<(double latitude, double longitude)> route)
    //{
    //    var polyline = new Polyline
    //    {
    //        StrokeWidth = 5,
    //        StrokeColor = Color.FromHex("#eee")
    //    };

    //    foreach (var position in route)
    //    {
    //        polyline.Geopath.Add(new Location(position.latitude, position.longitude));
    //    }

    //    map.MapElements.Add(polyline);
    //}

    //private List<(double latitude, double longitude)> DecodePolyline(string encodedPoints)
    //{
    //    var polylinePositions = new List<(double latitude, double longitude)>();
    //    int index = 0;
    //    int latitude = 0;
    //    int longitude = 0;

    //    while (index < encodedPoints.Length)
    //    {
    //        int b;
    //        int shift = 0;
    //        int result = 0;
    //        do
    //        {
    //            b = encodedPoints[index++] - 63;
    //            result |= (b & 0x1f) << shift;
    //            shift += 5;
    //        } while (b >= 0x20);
    //        int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
    //        latitude += dlat;

    //        shift = 0;
    //        result = 0;
    //        do
    //        {
    //            b = encodedPoints[index++] - 63;
    //            result |= (b & 0x1f) << shift;
    //            shift += 5;
    //        } while (b >= 0x20);
    //        int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
    //        longitude += dlng;

    //        double finalLat = latitude / 1E5;
    //        double finalLng = longitude / 1E5;

    //        polylinePositions.Add((finalLat, finalLng));
    //    }

    //    return polylinePositions;
    //}



    //#endregion
}