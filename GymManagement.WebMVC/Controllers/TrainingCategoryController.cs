using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class TrainingCategoryController : Controller
{
    private readonly GymContext _context;

    public TrainingCategoryController(GymContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.TrainingCategories.ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _context.TrainingCategories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainingCategory category)
    {
        if (!ModelState.IsValid) return View(category);
        _context.TrainingCategories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _context.TrainingCategories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrainingCategory category)
    {
        if (id != category.Id) return BadRequest();
        if (!ModelState.IsValid) return View(category);
        
        _context.TrainingCategories.Update(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.TrainingCategories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.TrainingCategories.FindAsync(id);
        if (category != null)
        {
            _context.TrainingCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}