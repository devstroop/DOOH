using Radzen;

namespace DOOH.Server.Extensions;

public static class GoogleMapPositionExtensions
{
    
    public static Tuple<GoogleMapPosition, int> GetCenterAndZoom(List<GoogleMapPosition> coordinates)
    {
        if (coordinates == null || !coordinates.Any())
        {
            throw new ArgumentException("The list of coordinates cannot be null or empty.");
        }

        // Calculate the geographic center (centroid)
        double latitudeSum = 0;
        double longitudeSum = 0;
        foreach (var coordinate in coordinates)
        {
            latitudeSum += coordinate.Lat;
            longitudeSum += coordinate.Lng;
        }

        double centerLatitude = latitudeSum / coordinates.Count;
        double centerLongitude = longitudeSum / coordinates.Count;

        // Calculate the zoom level
        // You can adjust the factor to get the desired zoom level, here is a simple heuristic
        var maxLat = coordinates.Max(c => c.Lat);
        var minLat = coordinates.Min(c => c.Lat);
        var maxLng = coordinates.Max(c => c.Lng);
        var minLng = coordinates.Min(c => c.Lng);

        double latDiff = maxLat - minLat;
        double lngDiff = maxLng - minLng;

        // Explanation of the formula:
        // - The factor 8 is a simple heuristic to get a zoom level that fits the bounding box
        // - The Math.Max is used to ensure that the zoom level is not negative
        // - The Math.Min is used to ensure that the zoom level is not greater than 21 (the maximum zoom level)
        //
        // int zoom = (int)Math.Floor(8 - Math.Log(Math.Max(latDiff, lngDiff)) / Math.Log(2)); // Not working as expected
        
        // Calculate zoom to fit the bounding box correctly
        double zoom = 0;
        if (latDiff > lngDiff)
        {
            zoom = Math.Log(360.0 / 256.0 * 180.0 / latDiff) / Math.Log(2);
        }
        else
        {
            zoom = Math.Log(180.0 / 256.0 * 360.0 / lngDiff) / Math.Log(2);
        }

        // Adjust zoom to be within a valid range (usually 0 to 21)
        zoom = Math.Max(0, Math.Min(zoom, 21));

        return new Tuple<GoogleMapPosition, int>(new GoogleMapPosition
        {
            Lat = centerLatitude,
            Lng = centerLongitude
        }, Convert.ToInt32(zoom));
    }
}