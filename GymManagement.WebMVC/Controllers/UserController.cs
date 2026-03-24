using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly GymContext _context;
    public UserController(GymContext context) => _context = context;

    [AllowAnonymous]
    public async Task<IActionResult> Index() =>
        View(await _context.Users
            .Include(u => u.Admin)
            .Include(u => u.Client)
            .Include(u => u.Trainer)
            .ToListAsync());

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var item = await _context.Users

            .Include(u => u.Admin)
            .Include(u => u.Client)
            .Include(u => u.Trainer).ThenInclude(t => t!.Specialization)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create()
    {
        ViewBag.Specializations = new SelectList(_context.Specializations, "Id", "Name");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user, string role, int? specializationId)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specializations = new SelectList(_context.Specializations, "Id", "Name");
            return View(user);
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        switch (role)
        {
            case "Admin":
                _context.Admins.Add(new Admin { UserId = user.Id });
                break;
            case "Trainer":
                _context.Trainers.Add(new Trainer { UserId = user.Id, SpecializationId = specializationId });
                break;
            case "Client":
                _context.Clients.Add(new Client { UserId = user.Id });
                break;
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.Users.FindAsync(id);
        if (item == null) return NotFound();
        ViewBag.Specializations = new SelectList(_context.Specializations, "Id", "Name");
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Specializations = new SelectList(_context.Specializations, "Id", "Name");
            return View(user);
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Users.FindAsync(id);
        if (item != null) { _context.Users.Remove(item); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}