using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task<IActionResult> Index(int carIndex)
        {
            var maintenanceRecords = await _context.MaintenanceRecords
                .Include(m => m.Car).Where(c => c.CarId == carIndex).ToListAsync();
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
        public async Task<IActionResult> Create(int carId)
        {
            ViewBag.carID = carId;
            Debug.Write(carId.ToString());
            return View("Edit", null);
        }

        // POST: MaintenanceRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceRecord maintenanceRecord, int carId)
        {
            if (ModelState.IsValid)
            {
                maintenanceRecord.CarId = carId;
                _context.Add(maintenanceRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Cars", new { id = carId });
            }
            ViewBag.carID = carId;
            return View("Edit",null);
        }

        // GET: MaintenanceRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            MaintenanceRecord? record = null;
            if (id != null)
            {
                record = await _context.MaintenanceRecords.SingleOrDefaultAsync(x => x.Id == id);
                if (record != null) ViewBag.carID = record.CarId;
            }
            return View(record);
        }

        //  POST: MaintenanceRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceRecord maintenanceRecord, int carId)
        {

            var record = await _context.MaintenanceRecords.AsNoTracking().SingleOrDefaultAsync(x => x.Id == maintenanceRecord.Id);
            if (record == null) return NotFound();

            maintenanceRecord.CarId = carId;
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
                return RedirectToAction("Edit", "Cars", new { id = carId });
            }
            return View(maintenanceRecord);
        }

      //  GET: MaintenanceRecords/Delete/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(id);
            if (maintenanceRecord != null)
            {
                _context.MaintenanceRecords.Remove(maintenanceRecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", "Cars", new { id = maintenanceRecord.CarId });
        }

        private bool MaintenanceRecordExists(int id)
        {
            return _context.MaintenanceRecords.Any(e => e.Id == id);
        }
    }
} 