using Microsoft.AspNetCore.Mvc;
using TNAI_Proj.Model;

namespace TNAI_Proj.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Cars");
        }
    }
} 