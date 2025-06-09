using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Cars
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
    {
        return await _context.Cars
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .ToListAsync();
    }

    // GET: api/Cars/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .Include(c => c.Reviews)
            .Include(c => c.MaintenanceRecords)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        return car;
    }

    // GET: api/Cars/available
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Car>>> GetAvailableCars()
    {
        return await _context.Cars
            .Where(c => c.IsAvailable)
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .ToListAsync();
    }

    // GET: api/Cars/category/5
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Car>>> GetCarsByCategory(int categoryId)
    {
        return await _context.Cars
            .Where(c => c.CategoryId == categoryId)
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .ToListAsync();
    }

    // POST: api/Cars
    [HttpPost]
    public async Task<ActionResult<Car>> CreateCar(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    // PUT: api/Cars/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, Car car)
    {
        if (id != car.Id)
        {
            return BadRequest();
        }

        _context.Entry(car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(id))
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

    // DELETE: api/Cars/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }
} 