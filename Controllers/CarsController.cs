using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNAI_Proj.Models;

public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cars
    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return View(cars);
    }

    // GET: Cars/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .Include(c => c.Category)
            .Include(c => c.Dealership)
            .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // GET: Cars/Create
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Dealerships = _context.Dealerships.ToList();
        return View();
    }

    // POST: Cars/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (ModelState.IsValid)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Dealerships = _context.Dealerships.ToList();
        return View(car);
    }

    // GET: Cars/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Dealerships = _context.Dealerships.ToList();
        return View(car);
    }

    // POST: Cars/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car)
    {
        if (id != car.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
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
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Dealerships = _context.Dealerships.ToList();
        return View(car);
    }

    // GET: Cars/Delete/5
    public async Task<IActionResult> Delete(int? id)
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

    // POST: Cars/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }
} 