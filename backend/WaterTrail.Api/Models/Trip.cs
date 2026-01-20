using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WaterTrail.Api.Models;

public enum TripType
{
    Kayak,
    Hiking,
    General
}

public class Trip
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TripType Type { get; set; } = TripType.Kayak;

    public List<Point> Points { get; set; } = new();
    public List<RoutePath> Paths { get; set; } = new();
    public List<Gear> Gear { get; set; } = new();
    public List<Food> Food { get; set; } = new();
    public List<Night> Nights { get; set; } = new();
}

public class TripCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TripType Type { get; set; } = TripType.Kayak;
    public List<PointCreateDto> Points { get; set; } = new();
    public List<PathCreateDto> Paths { get; set; } = new();
}

public class TripDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TripType Type { get; set; }
    public List<PointDto> Points { get; set; } = new();
    public List<PathDto> Paths { get; set; } = new();
    public List<GearDto> Gear { get; set; } = new();
    public List<FoodDto> Food { get; set; } = new();
    public List<NightDto> Nights { get; set; } = new();
}
