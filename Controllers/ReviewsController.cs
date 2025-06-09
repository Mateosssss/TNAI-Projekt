using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReviewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Car)
            .ToListAsync();
    }

    // GET: api/Reviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound();
        }

        return review;
    }

    // GET: api/Reviews/car/5
    [HttpGet("car/{carId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetCarReviews(int carId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.CarId == carId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // GET: api/Reviews/user/5
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetUserReviews(int userId)
    {
        return await _context.Reviews
            .Include(r => r.Car)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // POST: api/Reviews
    [HttpPost]
    public async Task<ActionResult<Review>> CreateReview(Review review)
    {
        // Verify if user has purchased the car
        var hasPurchased = await _context.Orders
            .AnyAsync(o => o.UserId == review.UserId && 
                          o.CarId == review.CarId && 
                          o.Status == "Completed");

        review.IsVerified = hasPurchased;
        review.CreatedAt = System.DateTime.UtcNow;

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
    }

    // PUT: api/Reviews/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, Review review)
    {
        if (id != review.Id)
        {
            return BadRequest();
        }

        review.UpdatedAt = System.DateTime.UtcNow;

        _context.Entry(review).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
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

    // DELETE: api/Reviews/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(int id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
} 