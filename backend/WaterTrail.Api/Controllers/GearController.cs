using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/trips/{tripId}/[controller]")]
public class GearController : ControllerBase
{
    private readonly AppDbContext _context;

    public GearController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<GearDto>>> GetGear(int tripId)
    {
        var gear = await _context.Gear
            .Where(g => g.TripId == tripId)
            .Select(g => new GearDto
            {
                Id = g.Id,
                TripId = g.TripId,
                Name = g.Name,
                Quantity = g.Quantity,
                Packed = g.Packed
            })
            .ToListAsync();

        return Ok(gear);
    }

    [HttpPost]
    public async Task<ActionResult<GearDto>> AddGear(int tripId, GearCreateDto gearDto)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists)
        {
            return NotFound("Trip not found");
        }

        var gear = new Gear
        {
            TripId = tripId,
            Name = gearDto.Name,
            Quantity = gearDto.Quantity,
            Packed = gearDto.Packed
        };

        _context.Gear.Add(gear);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGear), new { tripId }, new GearDto
        {
            Id = gear.Id,
            TripId = gear.TripId,
            Name = gear.Name,
            Quantity = gear.Quantity,
            Packed = gear.Packed
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGear(int tripId, int id, GearCreateDto gearDto)
    {
        var gear = await _context.Gear.FirstOrDefaultAsync(g => g.Id == id && g.TripId == tripId);
        if (gear == null)
        {
            return NotFound();
        }

        gear.Name = gearDto.Name;
        gear.Quantity = gearDto.Quantity;
        gear.Packed = gearDto.Packed;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGear(int tripId, int id)
    {
        var gear = await _context.Gear.FirstOrDefaultAsync(g => g.Id == id && g.TripId == tripId);
        if (gear == null)
        {
            return NotFound();
        }

        _context.Gear.Remove(gear);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
