using GymManagement.Domain.Entities;
using GymManagement.Infrastructure;
using GymManagement.WebMVC.Models;
using GymManagement.WebMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

[Authorize]
public class TrainerController : Controller
{
    private const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private readonly GymContext _context;
    private readonly IDataPortServiceFactory<TrainerRowDto> _portFactory;

    public TrainerController(GymContext context, IDataPortServiceFactory<TrainerRowDto> portFactory)
    {
        _context     = context;
        _portFactory = portFactory;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var trainers = await _context.Trainers
            .Include(t => t.User)
            .Include(t => t.Specialization)
            .ToListAsync();
        return View(trainers);
    }

    // ── Export ────────────────────────────────────────────────────────────────
    public async Task<IActionResult> Export()
    {
        var trainers = await _context.Trainers
            .Include(t => t.User)
            .Include(t => t.Specialization)
            .ToListAsync();

        var rows = trainers.Select(t => new TrainerRowDto
        {
            FirstName       = t.User.FirstName,
            LastName        = t.User.LastName,
            Email           = t.User.Email,
            Phone           = t.User.Phone,
            Specialization  = t.Specialization?.Name,
            ExperienceYears = t.ExperienceYears,
            HourlyRate      = t.HourlyRate,
        });

        var bytes = _portFactory.GetExportService(Xlsx).Export(rows);
        return File(bytes, Xlsx, $"trainers_{DateTime.Today:yyyy-MM-dd}.xlsx");
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
        var specDict = await _context.Specializations.ToDictionaryAsync(s => s.Name, s => s.Id);
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

            int? specId = row.Specialization != null && specDict.TryGetValue(row.Specialization, out var sid)
                ? sid : null;

            _context.Trainers.Add(new Trainer
            {
                UserId          = user.Id,
                SpecializationId = specId,
                ExperienceYears = row.ExperienceYears,
                HourlyRate      = row.HourlyRate,
                IsAvailable     = true,
            });
            await _context.SaveChangesAsync();

            existingEmails.Add(row.Email);
            count++;
        }

        TempData["Success"] = $"Імпортовано {count} тренер(ів).";
        return RedirectToAction(nameof(Index));
    }
}
