using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/trips/{tripId}/[controller]")]
public class PointsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PointsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PointDto>>> GetPoints(int tripId)
    {
        var points = await _context.Points
            .Where(p => p.TripId == tripId)
            .Select(p => new PointDto
            {
                Id = p.Id,
                TripId = p.TripId,
                Name = p.Name,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Icon = p.Icon,
                Color = p.Color
            })
            .ToListAsync();

        return Ok(points);
    }

    [HttpPost]
    public async Task<ActionResult<PointDto>> AddPoint(int tripId, PointCreateDto pointDto)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists)
        {
            return NotFound("Trip not found");
        }

        var point = new Point
        {
            TripId = tripId,
            Name = pointDto.Name,
            Latitude = pointDto.Latitude,
            Longitude = pointDto.Longitude,
            Icon = pointDto.Icon,
            Color = pointDto.Color
        };

        _context.Points.Add(point);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPoints), new { tripId }, new PointDto
        {
            Id = point.Id,
            TripId = point.TripId,
            Name = point.Name,
            Latitude = point.Latitude,
            Longitude = point.Longitude,
            Icon = point.Icon,
            Color = point.Color
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePoint(int tripId, int id)
    {
        var point = await _context.Points.FirstOrDefaultAsync(p => p.Id == id && p.TripId == tripId);
        if (point == null)
        {
            return NotFound();
        }

        _context.Points.Remove(point);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
