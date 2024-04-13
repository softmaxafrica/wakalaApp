using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Services
{
    public class LiveLocationService
    {
        private CancellationTokenSource _cancelTokenSource;
          private bool _isCheckingLocation;



    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            _cancelTokenSource = new CancellationTokenSource();
            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            return location;
        }
        catch (Exception ex)
        {
            // Unable to get location
            return null;  
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    }

  
}
