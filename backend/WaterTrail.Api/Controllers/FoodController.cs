using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Controllers;

[ApiController]
[Route("api/trips/{tripId}/[controller]")]
public class FoodController : ControllerBase
{
    private readonly AppDbContext _context;

    public FoodController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<FoodDto>>> GetFood(int tripId)
    {
        var food = await _context.Food
            .Where(f => f.TripId == tripId)
            .Select(f => new FoodDto
            {
                Id = f.Id,
                TripId = f.TripId,
                Name = f.Name,
                Quantity = f.Quantity,
                Eaten = f.Eaten
            })
            .ToListAsync();

        return Ok(food);
    }

    [HttpPost]
    public async Task<ActionResult<FoodDto>> AddFood(int tripId, FoodCreateDto foodDto)
    {
        var tripExists = await _context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists)
        {
            return NotFound("Trip not found");
        }

        var food = new Food
        {
            TripId = tripId,
            Name = foodDto.Name,
            Quantity = foodDto.Quantity,
            Eaten = foodDto.Eaten
        };

        _context.Food.Add(food);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFood), new { tripId }, new FoodDto
        {
            Id = food.Id,
            TripId = food.TripId,
            Name = food.Name,
            Quantity = food.Quantity,
            Eaten = food.Eaten
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFood(int tripId, int id, FoodCreateDto foodDto)
    {
        var food = await _context.Food.FirstOrDefaultAsync(f => f.Id == id && f.TripId == tripId);
        if (food == null)
        {
            return NotFound();
        }

        food.Name = foodDto.Name;
        food.Quantity = foodDto.Quantity;
        food.Eaten = foodDto.Eaten;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood(int tripId, int id)
    {
        var food = await _context.Food.FirstOrDefaultAsync(f => f.Id == id && f.TripId == tripId);
        if (food == null)
        {
            return NotFound();
        }

        _context.Food.Remove(food);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
