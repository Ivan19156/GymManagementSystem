using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class MembershipTypeController : Controller
{
    private readonly GymContext _context;
    public MembershipTypeController(GymContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.MembershipTypes.ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.MembershipTypes.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipType item)
    {
        if (!ModelState.IsValid) return View(item);
        _context.MembershipTypes.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.MembershipTypes.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MembershipType item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) return View(item);
        _context.MembershipTypes.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.MembershipTypes.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.MembershipTypes.FindAsync(id);
        if (item != null) { _context.MembershipTypes.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}