using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Car)
            .ToListAsync();
    }

    // GET: api/Orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Car)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // GET: api/Orders/user/5
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int userId)
    {
        return await _context.Orders
            .Include(o => o.Car)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    // POST: api/Orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        // Check if car is available
        var car = await _context.Cars.FindAsync(order.CarId);
        if (car == null || !car.IsAvailable)
        {
            return BadRequest("Car is not available for purchase");
        }

        // Set initial order status
        order.Status = "Pending";
        order.OrderDate = System.DateTime.UtcNow;

        _context.Orders.Add(order);
        
        // Mark car as unavailable
        car.IsAvailable = false;
        car.UserId = order.UserId;

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    // PUT: api/Orders/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        order.Status = status;
        
        if (status == "Completed")
        {
            order.CompletionDate = System.DateTime.UtcNow;
        }
        else if (status == "Cancelled" || status == "Refunded")
        {
            // Make car available again
            var car = await _context.Cars.FindAsync(order.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
                car.UserId = null;
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
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

    // DELETE: api/Orders/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(int id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }
} 