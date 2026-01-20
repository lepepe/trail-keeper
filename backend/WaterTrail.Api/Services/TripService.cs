using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Services;

public interface ITripService
{
    Task<List<TripDto>> GetAllTripsAsync();
    Task<TripDto?> GetTripByIdAsync(int id);
    Task<TripDto> CreateTripAsync(TripCreateDto tripDto);
    Task<bool> DeleteTripAsync(int id);
}

public class TripService : ITripService
{
    private readonly AppDbContext _context;

    public TripService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TripDto>> GetAllTripsAsync()
    {
        return await _context.Trips
            .Select(t => new TripDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Type = t.Type
            })
            .ToListAsync();
    }

    public async Task<TripDto?> GetTripByIdAsync(int id)
    {
        var trip = await _context.Trips
            .Include(t => t.Points)
            .Include(t => t.Paths)
            .Include(t => t.Gear)
            .Include(t => t.Food)
            .Include(t => t.Nights)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip == null) return null;

        return new TripDto
        {
            Id = trip.Id,
            Name = trip.Name,
            Description = trip.Description,
            Type = trip.Type,
            Points = trip.Points.Select(p => new PointDto
            {
                Id = p.Id,
                TripId = p.TripId,
                Name = p.Name,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Icon = p.Icon,
                Color = p.Color
            }).ToList(),
            Paths = trip.Paths.Select(p => new PathDto
            {
                Id = p.Id,
                TripId = p.TripId,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).ToList(),
            Gear = trip.Gear.Select(g => new GearDto
            {
                Id = g.Id,
                TripId = g.TripId,
                Name = g.Name,
                Quantity = g.Quantity,
                Packed = g.Packed
            }).ToList(),
            Food = trip.Food.Select(f => new FoodDto
            {
                Id = f.Id,
                TripId = f.TripId,
                Name = f.Name,
                Quantity = f.Quantity,
                Eaten = f.Eaten
            }).ToList(),
            Nights = trip.Nights.Select(n => new NightDto
            {
                Id = n.Id,
                TripId = n.TripId,
                NightNumber = n.NightNumber,
                Campsite = n.Campsite,
                Notes = n.Notes
            }).ToList()
        };
    }

    public async Task<TripDto> CreateTripAsync(TripCreateDto tripDto)
    {
        var trip = new Trip
        {
            Name = tripDto.Name,
            Description = tripDto.Description,
            Type = tripDto.Type
        };

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        foreach (var pointDto in tripDto.Points)
        {
            _context.Points.Add(new Point
            {
                TripId = trip.Id,
                Name = pointDto.Name,
                Latitude = pointDto.Latitude,
                Longitude = pointDto.Longitude,
                Icon = pointDto.Icon,
                Color = pointDto.Color
            });
        }

        foreach (var pathDto in tripDto.Paths)
        {
            _context.RoutePaths.Add(new RoutePath
            {
                TripId = trip.Id,
                Latitude = pathDto.Latitude,
                Longitude = pathDto.Longitude
            });
        }

        await _context.SaveChangesAsync();

        return (await GetTripByIdAsync(trip.Id))!;
    }

    public async Task<bool> DeleteTripAsync(int id)
    {
        var trip = await _context.Trips.FindAsync(id);
        if (trip == null) return false;

        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();
        return true;
    }
}
