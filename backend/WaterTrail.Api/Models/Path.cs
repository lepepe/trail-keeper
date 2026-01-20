using System.Text.Json.Serialization;

namespace WaterTrail.Api.Models;

public class RoutePath
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public Trip? Trip { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}

public class PathCreateDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class PathDto
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
