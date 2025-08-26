using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNAI_Proj.Model.Entities;

namespace TNAI_Proj.Controllers
{
    public class MaintenanceRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MaintenanceRecords
        public async Task<IActionResult> Index()
        {
            var maintenanceRecords = await _context.MaintenanceRecords
                .Include(m => m.Car)
                .ToListAsync();
            return View(maintenanceRecords);
        }

        // GET: MaintenanceRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            return View(maintenanceRecord);
        }

        // GET: MaintenanceRecords/Create
        public IActionResult Create()
        {
            ViewBag.Cars = _context.Cars.ToList();
            return View();
        }

        // POST: MaintenanceRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRecord maintenanceRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cars = _context.Cars.ToList();
            return View(maintenanceRecord);
        }

        // GET: MaintenanceRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(id);
            if (maintenanceRecord == null)
            {
                return NotFound();
            }
            ViewBag.Cars = _context.Cars.ToList();
            return View(maintenanceRecord);
        }

        // POST: MaintenanceRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaintenanceRecord maintenanceRecord)
        {
            if (id != maintenanceRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceRecordExists(maintenanceRecord.Id))
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
            ViewBag.Cars = _context.Cars.ToList();
            return View(maintenanceRecord);
        }

        // GET: MaintenanceRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            return View(maintenanceRecord);
        }

        // POST: MaintenanceRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(id);
            if (maintenanceRecord != null)
            {
                _context.MaintenanceRecords.Remove(maintenanceRecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceRecordExists(int id)
        {
            return _context.MaintenanceRecords.Any(e => e.Id == id);
        }
    }
} 