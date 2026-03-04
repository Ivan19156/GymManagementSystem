using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class ScheduledSessionController : Controller
{
    private readonly GymContext _context;
    public ScheduledSessionController(GymContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.ScheduledSessions
            .Include(s => s.Client).ThenInclude(c => c.User)
            .Include(s => s.Trainer).ThenInclude(t => t.User)
            .Include(s => s.TrainingType)
            .Include(s => s.Status)
            .ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.ScheduledSessions
            .Include(s => s.Client).ThenInclude(c => c.User)
            .Include(s => s.Trainer).ThenInclude(t => t.User)
            .Include(s => s.TrainingType)
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    private void PopulateDropdowns(ScheduledSession? item = null)
    {
        ViewBag.Clients = new SelectList(
            _context.Clients.Include(c => c.User).Select(c => new { c.UserId, Name = c.User.FirstName + " " + c.User.LastName }),
            "UserId", "Name", item?.ClientId);
        ViewBag.Trainers = new SelectList(
            _context.Trainers.Include(t => t.User).Select(t => new { t.UserId, Name = t.User.FirstName + " " + t.User.LastName }),
            "UserId", "Name", item?.TrainerId);
        ViewBag.TrainingTypes = new SelectList(_context.TrainingTypes, "Id", "Name", item?.TrainingTypeId);
        ViewBag.Statuses = new SelectList(_context.SessionStatuses, "Id", "Name", item?.StatusId);
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduledSession item)
    {
        if (!ModelState.IsValid) { PopulateDropdowns(item); return View(item); }
        _context.ScheduledSessions.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.ScheduledSessions.FindAsync(id);
        if (item == null) return NotFound();
        PopulateDropdowns(item);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ScheduledSession item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) { PopulateDropdowns(item); return View(item); }
        _context.ScheduledSessions.Update(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ScheduledSessions
            .Include(s => s.Client).ThenInclude(c => c.User)
            .Include(s => s.Trainer).ThenInclude(t => t.User)
            .Include(s => s.TrainingType)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.ScheduledSessions.FindAsync(id);
        if (item != null) { _context.ScheduledSessions.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}