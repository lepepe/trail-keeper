using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/trips/{tripId}/[controller]")]
public class NightsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NightsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<NightDto>>> GetNights(int tripId)
    {
        var nights = await _context.Nights
            .Where(n => n.TripId == tripId)
            .OrderBy(n => n.NightNumber)
            .Select(n => new NightDto
            {
                Id = n.Id,
                TripId = n.TripId,
                NightNumber = n.NightNumber,
                Campsite = n.Campsite,
                Notes = n.Notes
            })
            .ToListAsync();

        return Ok(nights);
    }

    [HttpPost]
    public async Task<ActionResult<NightDto>> AddNight(int tripId, NightCreateDto nightDto)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists)
        {
            return NotFound("Trip not found");
        }

        var night = new Night
        {
            TripId = tripId,
            NightNumber = nightDto.NightNumber,
            Campsite = nightDto.Campsite,
            Notes = nightDto.Notes
        };

        _context.Nights.Add(night);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNights), new { tripId }, new NightDto
        {
            Id = night.Id,
            TripId = night.TripId,
            NightNumber = night.NightNumber,
            Campsite = night.Campsite,
            Notes = night.Notes
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNight(int tripId, int id, NightCreateDto nightDto)
    {
        var night = await _context.Nights.FirstOrDefaultAsync(n => n.Id == id && n.TripId == tripId);
        if (night == null)
        {
            return NotFound();
        }

        night.NightNumber = nightDto.NightNumber;
        night.Campsite = nightDto.Campsite;
        night.Notes = nightDto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNight(int tripId, int id)
    {
        var night = await _context.Nights.FirstOrDefaultAsync(n => n.Id == id && n.TripId == tripId);
        if (night == null)
        {
            return NotFound();
        }

        _context.Nights.Remove(night);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
