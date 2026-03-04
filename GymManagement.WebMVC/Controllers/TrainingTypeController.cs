using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class TrainingTypeController : Controller
{
    private readonly GymContext _context;
    public TrainingTypeController(GymContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.TrainingTypes.Include(t => t.Category).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.TrainingTypes.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.TrainingCategories, "Id", "Name");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainingType item)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_context.TrainingCategories, "Id", "Name");
            return View(item);
        }
        _context.TrainingTypes.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.TrainingTypes.FindAsync(id);
        if (item == null) return NotFound();
        ViewBag.Categories = new SelectList(_context.TrainingCategories, "Id", "Name", item.CategoryId);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrainingType item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_context.TrainingCategories, "Id", "Name", item.CategoryId);
            return View(item);
        }
        _context.TrainingTypes.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.TrainingTypes.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.TrainingTypes.FindAsync(id);
        if (item != null) { _context.TrainingTypes.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}