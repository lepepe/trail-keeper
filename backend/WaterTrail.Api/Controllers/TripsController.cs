using Microsoft.AspNetCore.Mvc;
using WaterTrail.Api.Models;
using WaterTrail.Api.Services;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TripDto>>> GetTrips()
    {
        var trips = await _tripService.GetAllTripsAsync();
        return Ok(trips);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripDto>> GetTrip(int id)
    {
        var trip = await _tripService.GetTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();
        }
        return Ok(trip);
    }

    [HttpPost]
    public async Task<ActionResult<TripDto>> CreateTrip(TripCreateDto tripDto)
    {
        var trip = await _tripService.CreateTripAsync(tripDto);
        return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var result = await _tripService.DeleteTripAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
