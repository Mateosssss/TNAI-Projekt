using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DealershipsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DealershipsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Dealerships
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dealership>>> GetDealerships()
    {
        return await _context.Dealerships
            .Include(d => d.Cars)
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    // GET: api/Dealerships/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Dealership>> GetDealership(int id)
    {
        var dealership = await _context.Dealerships
            .Include(d => d.Cars)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dealership == null)
        {
            return NotFound();
        }

        return dealership;
    }

    // GET: api/Dealerships/5/cars
    [HttpGet("{id}/cars")]
    public async Task<ActionResult<IEnumerable<Car>>> GetDealershipCars(int id)
    {
        var dealership = await _context.Dealerships
            .Include(d => d.Cars)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dealership == null)
        {
            return NotFound();
        }

        return dealership.Cars.ToList();
    }

    // GET: api/Dealerships/5/cars/available
    [HttpGet("{id}/cars/available")]
    public async Task<ActionResult<IEnumerable<Car>>> GetDealershipAvailableCars(int id)
    {
        var dealership = await _context.Dealerships
            .Include(d => d.Cars.Where(c => c.IsAvailable))
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dealership == null)
        {
            return NotFound();
        }

        return dealership.Cars.ToList();
    }

    // POST: api/Dealerships
    [HttpPost]
    public async Task<ActionResult<Dealership>> CreateDealership(Dealership dealership)
    {
        dealership.CreatedAt = System.DateTime.UtcNow;
        dealership.IsActive = true;

        _context.Dealerships.Add(dealership);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDealership), new { id = dealership.Id }, dealership);
    }

    // PUT: api/Dealerships/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDealership(int id, Dealership dealership)
    {
        if (id != dealership.Id)
        {
            return BadRequest();
        }

        dealership.UpdatedAt = System.DateTime.UtcNow;

        _context.Entry(dealership).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DealershipExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Dealerships/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDealership(int id)
    {
        var dealership = await _context.Dealerships.FindAsync(id);
        if (dealership == null)
        {
            return NotFound();
        }

        // Soft delete - mark as inactive instead of removing
        dealership.IsActive = false;
        dealership.UpdatedAt = System.DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DealershipExists(int id)
    {
        return _context.Dealerships.Any(e => e.Id == id);
    }
} 