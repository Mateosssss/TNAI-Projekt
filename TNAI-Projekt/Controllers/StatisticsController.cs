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
        public IActionResult Statistics()
        {
            ViewBag.Labels = new[] { "Styczeñ", "Luty", "Marzec" };
            ViewBag.Values = new[] { 1200, 1900, 3000 };
            return View();
        }
    }
} 