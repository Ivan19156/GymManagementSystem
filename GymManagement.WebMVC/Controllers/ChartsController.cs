using GymManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.WebMVC.Controllers;

public class ChartsController : Controller
{
    private readonly GymContext _context;
    public ChartsController(GymContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> MembershipsByMonth()
    {
        var dates = await _context.UserMemberships
            .Select(m => m.StartDate)
            .ToListAsync();

        var grouped = dates
            .GroupBy(d => new { d.Year, d.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        return Json(new
        {
            labels = grouped.Select(x => $"{x.Month:D2}/{x.Year}"),
            data   = grouped.Select(x => x.Count)
        });
    }

    [HttpGet]
    public async Task<IActionResult> SessionsByTrainer()
    {
        var sessions = await _context.ScheduledSessions
            .Where(s => s.TrainerId != null)
            .Include(s => s.Trainer).ThenInclude(t => t!.User)
            .ToListAsync();

        var grouped = sessions
            .GroupBy(s => s.TrainerId)
            .Select(g => new
            {
                Name  = g.First().Trainer!.User.FirstName + " " + g.First().Trainer!.User.LastName,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        return Json(new
        {
            labels = grouped.Select(x => x.Name),
            data   = grouped.Select(x => x.Count)
        });
    }
}
