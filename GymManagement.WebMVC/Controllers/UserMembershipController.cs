using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

[Authorize]
public class UserMembershipController : Controller
{
    private readonly GymContext _context;
    public UserMembershipController(GymContext context) => _context = context;

    [AllowAnonymous]
    public async Task<IActionResult> Index() =>
        View(await _context.UserMemberships
            .Include(m => m.Client).ThenInclude(c => c.User)
            .Include(m => m.MembershipPlan).ThenInclude(p => p.Type)
            .Include(m => m.Status)
            .ToListAsync());

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.UserMemberships
            .Include(m => m.Client).ThenInclude(c => c.User)
            .Include(m => m.MembershipPlan).ThenInclude(p => p.Type)
            .Include(m => m.Status)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    private void PopulateDropdowns(UserMembership? item = null)
    {
        ViewBag.Clients = new SelectList(
            _context.Clients.Include(c => c.User).Select(c => new { c.UserId, Name = c.User.FirstName + " " + c.User.LastName }),
            "UserId", "Name", item?.ClientId);
        ViewBag.Plans = new SelectList(_context.MembershipPlans.Include(p => p.Type)
            .Select(p => new { p.Id, Name = p.Name + " (" + p.Type.Name + ")" }),
            "Id", "Name", item?.MembershipPlanId);
        ViewBag.Statuses = new SelectList(_context.MembershipStatuses, "Id", "Name", item?.StatusId);
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserMembership item)
    {
        if (!ModelState.IsValid) { PopulateDropdowns(item); return View(item); }
        _context.UserMemberships.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.UserMemberships.FindAsync(id);
        if (item == null) return NotFound();
        PopulateDropdowns(item);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserMembership item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) { PopulateDropdowns(item); return View(item); }
        _context.UserMemberships.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.UserMemberships
            .Include(m => m.Client).ThenInclude(c => c.User)
            .Include(m => m.MembershipPlan)
            .Include(m => m.Status)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.UserMemberships.FindAsync(id);
        if (item != null) { _context.UserMemberships.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}