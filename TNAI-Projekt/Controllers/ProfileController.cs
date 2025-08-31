using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TNAI_Proj.Model.Entities.Auth;
using TNAI_Proj.Model.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace TNAI_Proj.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Profile/Settings
        public IActionResult Update()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.SingleOrDefault(x => x.Id == int.Parse(userId));
            if (user == null) return NotFound();


            var profile = new UpdateModel()
            {
                Address = user.Address,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            return View(profile);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.SingleOrDefault(x => x.Id == int.Parse(userId));
                if (user == null) return NotFound();

                if (user.Email != model.Email)
                {
                    if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "Email already in use.");
                        return View(model);
                    }
                }     

                user.Name = model.Name;
                user.Email = model.Email;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
} 