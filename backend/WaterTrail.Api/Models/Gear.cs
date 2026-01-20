using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WaterTrail.Api.Models;

public class Gear
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public Trip? Trip { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    public bool Packed { get; set; } = false;
}

public class GearCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public bool Packed { get; set; } = false;
}

public class GearDto
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool Packed { get; set; }
}
