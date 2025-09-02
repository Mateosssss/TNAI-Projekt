using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNAI_Proj.Data;
using TNAI_Proj.Model;
using System.Linq;
using System.Threading.Tasks;

namespace TNAI_Proj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Statistics(int range)
        {
            if(range <= 0) return NotFound();
            Statistics statistics = new Statistics();

            statistics.range = range;
            var today = DateTime.UtcNow.Date;
            var sevenDaysAgo = today.AddDays(-(range -1));
            var allDates = new List<DateTime>();
            for (int i = 0; i < range; i++)
            {
                allDates.Add(sevenDaysAgo.AddDays(i));
            }

            var ordersPerDay = _context.Orders
                .Where(o => o.OrderDate >= sevenDaysAgo)
                .GroupBy(o => o.OrderDate.Date) 
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            var result = allDates
            .Select(d => new
            {
                Date = d,
                Count = ordersPerDay.FirstOrDefault(x => x.Date == d)?.Count ?? 0
            })
            .ToList();

            ViewBag.Labels = result.Select(x => x.Date.ToString("dd.MM.yyyy")).ToList();
            ViewBag.Values = result.Select(x => x.Count).ToList();

            statistics.newOrders = _context.Orders
                .Where(o => o.OrderDate >= sevenDaysAgo).Count();
            statistics.newUsers = _context.Users
            .Where(o => o.CreatedAt >= sevenDaysAgo).Count();
            statistics.newCars = _context.Cars
            .Where(o => o.CreatedAt >= sevenDaysAgo).Count();
            statistics.newReviews = _context.Reviews
            .Where(o => o.CreatedAt >= sevenDaysAgo).Count();

            return View(statistics);
        }
    }
} 