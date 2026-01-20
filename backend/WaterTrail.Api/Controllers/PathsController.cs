using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/trips/{tripId}/[controller]")]
public class PathsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PathsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PathDto>>> GetPaths(int tripId)
    {
        var paths = await _context.RoutePaths
            .Where(p => p.TripId == tripId)
            .Select(p => new PathDto
            {
                Id = p.Id,
                TripId = p.TripId,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            })
            .ToListAsync();

        return Ok(paths);
    }

    [HttpPost]
    public async Task<ActionResult<PathDto>> AddPath(int tripId, PathCreateDto pathDto)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists)
        {
            return NotFound("Trip not found");
        }

        var path = new RoutePath
        {
            TripId = tripId,
            Latitude = pathDto.Latitude,
            Longitude = pathDto.Longitude
        };

        _context.RoutePaths.Add(path);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaths), new { tripId }, new PathDto
        {
            Id = path.Id,
            TripId = path.TripId,
            Latitude = path.Latitude,
            Longitude = path.Longitude
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePath(int tripId, int id)
    {
        var path = await _context.RoutePaths.FirstOrDefaultAsync(p => p.Id == id && p.TripId == tripId);
        if (path == null)
        {
            return NotFound();
        }

        _context.RoutePaths.Remove(path);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
