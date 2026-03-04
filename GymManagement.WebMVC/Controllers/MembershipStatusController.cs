using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class MembershipStatusController : Controller
{
    private readonly GymContext _context;
    public MembershipStatusController(GymContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.MembershipStatuses.ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.MembershipStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipStatus item)
    {
        if (!ModelState.IsValid) return View(item);
        _context.MembershipStatuses.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.MembershipStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MembershipStatus item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) return View(item);
        _context.MembershipStatuses.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.MembershipStatuses.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.MembershipStatuses.FindAsync(id);
        if (item != null) { _context.MembershipStatuses.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}