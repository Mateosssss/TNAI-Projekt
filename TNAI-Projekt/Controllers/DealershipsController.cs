using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNAI_Proj.Model.Entities;

namespace TNAI_Proj.Controllers
{
    public class DealershipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DealershipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dealerships
        public async Task<IActionResult> Index()
        {
            var dealerships = await _context.Dealerships
                .Include(d => d.Cars)
                .ToListAsync();
            return View(dealerships);
        }

        // GET: Dealerships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships
                .Include(d => d.Cars)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dealership == null)
            {
                return NotFound();
            }

            return View(dealership);
        }

        // GET: Dealerships/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dealerships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dealership dealership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dealership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dealership);
        }

        // GET: Dealerships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null)
            {
                return NotFound();
            }
            return View(dealership);
        }

        // POST: Dealerships/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dealership dealership)
        {
            if (id != dealership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dealership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealershipExists(dealership.Id))
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
            return View(dealership);
        }

        // GET: Dealerships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships
                .Include(d => d.Cars)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dealership == null)
            {
                return NotFound();
            }

            return View(dealership);
        }

        // POST: Dealerships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership != null)
            {
                _context.Dealerships.Remove(dealership);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DealershipExists(int id)
        {
            return _context.Dealerships.Any(e => e.Id == id);
        }
    }
} 