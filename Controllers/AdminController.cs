using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNAI_Proj.Data;
using TNAI_Proj.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TNAI_Proj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Include(u => u.BoughtCars)
                .ToListAsync();
            return View(users);
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            if (status == "Completed")
            {
                order.CompletionDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Orders));
        }

        // GET: Admin/Cars
        public async Task<IActionResult> Cars()
        {
            var cars = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Dealership)
                .Include(c => c.User)
                .ToListAsync();
            return View(cars);
        }

        // GET: Admin/Reviews
        public async Task<IActionResult> Reviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Car)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(reviews);
        }

        // POST: Admin/VerifyReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            review.IsVerified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reviews));
        }

        // GET: Admin/DeleteUser
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.BoughtCars)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/DeleteUser
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/DeleteCar
        public async Task<IActionResult> DeleteCar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Dealership)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Admin/DeleteCar
        [HttpPost, ActionName("DeleteCar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Cars));
        }
    }
} 