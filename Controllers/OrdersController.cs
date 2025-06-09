using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNAI_Proj.Models;
using System.Security.Claims;

namespace TNAI_Proj.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .Where(o => o.UserId.ToString() == userId)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId.ToString() == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(int carId)
        {
            var car = await _context.Cars
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == carId && c.IsAvailable);

            if (car == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                CarId = carId,
                TotalAmount = car.Price,
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                order.UserId = int.Parse(userId);
                order.OrderDate = DateTime.UtcNow;
                order.Status = "Pending";

                var car = await _context.Cars.FindAsync(order.CarId);
                if (car == null || !car.IsAvailable)
                {
                    ModelState.AddModelError("", "The selected car is no longer available.");
                    return View(order);
                }

                car.IsAvailable = false;
                _context.Update(car);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId.ToString() == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId.ToString() != userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId.ToString() == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.Car)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId.ToString() == userId);

            if (order != null)
            {
                if (order.Car != null)
                {
                    order.Car.IsAvailable = true;
                    _context.Update(order.Car);
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Orders/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders
                .Include(o => o.Car)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Only allow valid status values
            if (status != "Pending" && status != "Completed" && status != "Cancelled" && status != "Refunded")
            {
                return BadRequest("Invalid status");
            }

            order.Status = status;
            
            // If completing the order, set the completion date
            if (status == "Completed")
            {
                order.CompletionDate = DateTime.UtcNow;
            }
            // If cancelling or refunding, make the car available again
            else if (status == "Cancelled" || status == "Refunded")
            {
                if (order.Car != null)
                {
                    order.Car.IsAvailable = true;
                    _context.Update(order.Car);
                }
            }

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
} 