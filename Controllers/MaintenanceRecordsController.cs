using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceRecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MaintenanceRecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/MaintenanceRecords
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetMaintenanceRecords()
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Car)
            .OrderByDescending(m => m.MaintenanceDate)
            .ToListAsync();
    }

    // GET: api/MaintenanceRecords/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceRecord>> GetMaintenanceRecord(int id)
    {
        var maintenanceRecord = await _context.MaintenanceRecords
            .Include(m => m.Car)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (maintenanceRecord == null)
        {
            return NotFound();
        }

        return maintenanceRecord;
    }

    // GET: api/MaintenanceRecords/car/5
    [HttpGet("car/{carId}")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetCarMaintenanceRecords(int carId)
    {
        return await _context.MaintenanceRecords
            .Where(m => m.CarId == carId)
            .OrderByDescending(m => m.MaintenanceDate)
            .ToListAsync();
    }

    // GET: api/MaintenanceRecords/car/5/summary
    [HttpGet("car/{carId}/summary")]
    public async Task<ActionResult<object>> GetCarMaintenanceSummary(int carId)
    {
        var records = await _context.MaintenanceRecords
            .Where(m => m.CarId == carId)
            .ToListAsync();

        var summary = new
        {
            TotalCost = records.Sum(r => r.Cost),
            LastMaintenanceDate = records.Max(r => r.MaintenanceDate),
            ServiceCount = records.Count,
            ServiceTypes = records.GroupBy(r => r.ServiceType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
        };

        return summary;
    }

    // POST: api/MaintenanceRecords
    [HttpPost]
    public async Task<ActionResult<MaintenanceRecord>> CreateMaintenanceRecord(MaintenanceRecord record)
    {
        record.CreatedAt = System.DateTime.UtcNow;

        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMaintenanceRecord), new { id = record.Id }, record);
    }

    // PUT: api/MaintenanceRecords/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceRecord(int id, MaintenanceRecord record)
    {
        if (id != record.Id)
        {
            return BadRequest();
        }

        record.UpdatedAt = System.DateTime.UtcNow;

        _context.Entry(record).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MaintenanceRecordExists(id))
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

    // DELETE: api/MaintenanceRecords/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceRecord(int id)
    {
        var record = await _context.MaintenanceRecords.FindAsync(id);
        if (record == null)
        {
            return NotFound();
        }

        _context.MaintenanceRecords.Remove(record);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MaintenanceRecordExists(int id)
    {
        return _context.MaintenanceRecords.Any(e => e.Id == id);
    }
} 