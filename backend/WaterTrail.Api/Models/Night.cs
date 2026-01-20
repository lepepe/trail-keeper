using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WaterTrail.Api.Models;

public class Night
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public Trip? Trip { get; set; }

    public int NightNumber { get; set; }

    [MaxLength(200)]
    public string? Campsite { get; set; }

    public string? Notes { get; set; }
}

public class NightCreateDto
{
    public int NightNumber { get; set; }
    public string? Campsite { get; set; }
    public string? Notes { get; set; }
}

public class NightDto
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public int NightNumber { get; set; }
    public string? Campsite { get; set; }
    public string? Notes { get; set; }
}
