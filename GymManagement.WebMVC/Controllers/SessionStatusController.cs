using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class SessionStatusController : Controller
{
    private readonly GymContext _context;
    public SessionStatusController(GymContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.SessionStatuses.ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.SessionStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SessionStatus item)
    {
        if (!ModelState.IsValid) return View(item);
        _context.SessionStatuses.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.SessionStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SessionStatus item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) return View(item);
        _context.SessionStatuses.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.SessionStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.SessionStatuses.FindAsync(id);
        if (item != null) { _context.SessionStatuses.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}