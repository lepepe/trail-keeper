using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WaterTrail.Api.Models;

public class Point
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public Trip? Trip { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [MaxLength(50)]
    public string? Icon { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }
}

public class PointCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
}

public class PointDto
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
}
