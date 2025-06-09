using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TNAI_Proj.Data;
using TNAI_Proj.Models;

namespace TNAI_Proj.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(int? carId)
        {
            if (carId.HasValue)
            {
                var car = await _context.Cars
                    .Include(c => c.Category)
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(c => c.Id == carId);

                if (car == null)
                {
                    return NotFound();
                }

                return View("CarReviews", car);
            }
            else
            {
                // Get all reviews for the current user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Car)
                    .Where(r => r.UserId.ToString() == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                return View("MyReviews", reviews);
            }
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create(int carId)
        {
            // Check if user has purchased the car
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.CarId == carId && 
                                        o.UserId.ToString() == userId && 
                                        o.Status == "Completed");

            if (order == null)
            {
                return NotFound("You can only review cars you have purchased.");
            }

            // Check if user has already reviewed this car
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.CarId == carId && 
                                        r.UserId.ToString() == userId);

            if (existingReview != null)
            {
                return RedirectToAction("Edit", new { id = existingReview.Id });
            }

            ViewBag.OrderId = order.Id;
            var review = new Review
            {
                CarId = carId,
                UserId = int.Parse(userId),
                CreatedAt = DateTime.UtcNow
            };

            return View(review);
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.UtcNow;
                review.IsVerified = false; // Reviews start unverified
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Orders", new { id = ViewBag.OrderId });
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (review.UserId.ToString() != userId)
            {
                return Forbid();
            }

            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.UpdatedAt = DateTime.UtcNow;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Orders", new { id = ViewBag.OrderId });
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
} 