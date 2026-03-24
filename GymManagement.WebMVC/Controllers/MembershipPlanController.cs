using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

[Authorize]
public class MembershipPlanController : Controller
{
    private readonly GymContext _context;
    public MembershipPlanController(GymContext context) => _context = context;

    [AllowAnonymous]
    public async Task<IActionResult> Index() =>
        View(await _context.MembershipPlans.Include(m => m.Type).ToListAsync());

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.MembershipPlans.Include(m => m.Type).FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create()
    {
        ViewBag.Types = new SelectList(_context.MembershipTypes, "Id", "Name");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipPlan item)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Types = new SelectList(_context.MembershipTypes, "Id", "Name");
            return View(item);
        }
        _context.MembershipPlans.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.MembershipPlans.FindAsync(id);
        if (item == null) return NotFound();
        ViewBag.Types = new SelectList(_context.MembershipTypes, "Id", "Name", item.TypeId);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MembershipPlan item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Types = new SelectList(_context.MembershipTypes, "Id", "Name", item.TypeId);
            return View(item);
        }
        _context.MembershipPlans.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.MembershipPlans.Include(m => m.Type).FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.MembershipPlans.FindAsync(id);
        if (item != null) { _context.MembershipPlans.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}