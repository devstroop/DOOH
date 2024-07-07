namespace OpenLayer.NET.Models;

public class Location(string label, double latitude = 28.644800, double longitude = 77.216721)
{
    public string Label { get; set; } = label;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
}