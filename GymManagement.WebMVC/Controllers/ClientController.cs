using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using GymManagement.WebMVC.Models;
using GymManagement.WebMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

[Authorize]
public class ClientController : Controller
{
    private const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private readonly GymContext _context;
    private readonly IDataPortServiceFactory<ClientRowDto> _portFactory;

    public ClientController(GymContext context, IDataPortServiceFactory<ClientRowDto> portFactory)
    {
        _context     = context;
        _portFactory = portFactory;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var clients = await _context.Clients
            .Include(c => c.User)
            .ToListAsync();
        return View(clients);
    }

    // ── Export ────────────────────────────────────────────────────────────────
    public async Task<IActionResult> Export()
    {
        var clients = await _context.Clients.Include(c => c.User).ToListAsync();
        var rows = clients.Select(c => new ClientRowDto
        {
            FirstName    = c.User.FirstName,
            LastName     = c.User.LastName,
            Email        = c.User.Email,
            Phone        = c.User.Phone,
            MedicalNotes = c.MedicalNotes,
        });

        var bytes = _portFactory.GetExportService(Xlsx).Export(rows);
        return File(bytes, Xlsx, $"clients_{DateTime.Today:yyyy-MM-dd}.xlsx");
    }

    // ── Import ────────────────────────────────────────────────────────────────
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            TempData["Error"] = "Файл не обрано.";
            return RedirectToAction(nameof(Index));
        }

        var rows = _portFactory.GetImportService(Xlsx).Import(file.OpenReadStream()).ToList();
        var existingEmails = await _context.Users.Select(u => u.Email).ToHashSetAsync();
        int count = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Email) || existingEmails.Contains(row.Email))
                continue;

            var user = new User
            {
                Email     = row.Email,
                FirstName = row.FirstName,
                LastName  = row.LastName,
                Phone     = row.Phone,
                IsActive  = true,
                CreatedAt = DateTime.Now,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.Clients.Add(new Client { UserId = user.Id, MedicalNotes = row.MedicalNotes });
            await _context.SaveChangesAsync();

            existingEmails.Add(row.Email);
            count++;
        }

        TempData["Success"] = $"Імпортовано {count} клієнт(ів).";
        return RedirectToAction(nameof(Index));
    }
}
