using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(GymContext ctx)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var now   = DateTime.Now;

        // ── 1. Specializations ────────────────────────────────────────────────
        var specNames = new[] { "Йога", "Силові тренування", "Кардіо", "Пілатес", "Бокс" };
        await UpsertByName(ctx.Specializations, specNames, n => new Specialization { Name = n }, ctx);
        var specs = await ctx.Specializations.Where(s => specNames.Contains(s.Name)).ToListAsync();

        // ── 2. MembershipStatuses ─────────────────────────────────────────────
        var msNames = new[] { "Активний", "Закінчився", "Призупинений" };
        await UpsertByName(ctx.MembershipStatuses, msNames, n => new MembershipStatus { Name = n }, ctx);
        var msDict = await ctx.MembershipStatuses.Where(s => msNames.Contains(s.Name))
                              .ToDictionaryAsync(s => s.Name);

        // ── 3. SessionStatuses ────────────────────────────────────────────────
        var ssNames = new[] { "Заплановано", "Завершено", "Скасовано" };
        await UpsertByName(ctx.SessionStatuses, ssNames, n => new SessionStatus { Name = n }, ctx);
        var ssDict = await ctx.SessionStatuses.Where(s => ssNames.Contains(s.Name))
                              .ToDictionaryAsync(s => s.Name);

        // ── 4. Users ──────────────────────────────────────────────────────────
        var userData = new[]
        {
            ("o.koval@gym.ua",      "Олексій", "Коваль",     "+380501234567", true,  now.AddMonths(-12)),
            ("m.shevchenko@gym.ua", "Марія",   "Шевченко",   "+380502345678", true,  now.AddMonths(-10)),
            ("i.bondarenko@gym.ua", "Іван",    "Бондаренко", "+380503456789", true,  now.AddMonths(-8) ),
            ("n.petrenko@gym.ua",   "Наталія", "Петренко",   "+380504567890", true,  now.AddMonths(-6) ),
            ("d.melnyk@gym.ua",     "Дмитро",  "Мельник",    "+380505678901", true,  now.AddMonths(-5) ),
            ("i.lysenko@gym.ua",    "Ірина",   "Лисенко",    "+380506789012", true,  now.AddMonths(-4) ),
            ("a.polischuk@gym.ua",  "Андрій",  "Поліщук",    "+380507890123", true,  now.AddMonths(-3) ),
            ("t.savchenko@gym.ua",  "Тетяна",  "Савченко",   "+380508901234", false, now.AddMonths(-7) ),
            ("s.kravchenko@gym.ua", "Сергій",  "Кравченко",  "+380509012345", true,  now.AddMonths(-14)),
            ("o.danchenko@gym.ua",  "Оксана",  "Данченко",   "+380500123456", false, now.AddMonths(-2) ),
        };
        var existingEmails = await ctx.Users.Select(u => u.Email).ToHashSetAsync();
        var newUsers = userData
            .Where(d => !existingEmails.Contains(d.Item1))
            .Select(d => new User
            {
                Email = d.Item1, FirstName = d.Item2, LastName = d.Item3,
                Phone = d.Item4, IsActive = d.Item5, CreatedAt = d.Item6
            }).ToList();
        if (newUsers.Count > 0) { ctx.Users.AddRange(newUsers); await ctx.SaveChangesAsync(); }

        var allEmails = userData.Select(d => d.Item1).ToArray();
        var users = await ctx.Users.Where(u => allEmails.Contains(u.Email))
                             .OrderBy(u => u.CreatedAt).ToListAsync();
        // Map by email for stable FK resolution
        var uByEmail = users.ToDictionary(u => u.Email);

        // ── 5. Trainers ───────────────────────────────────────────────────────
        var trainerData = new[]
        {
            (uByEmail["o.koval@gym.ua"].Id,      "Йога",              7,  600m,  true,  "Сертифікований інструктор з йоги, спеціалізується на хатха та віньяса йозі."),
            (uByEmail["m.shevchenko@gym.ua"].Id,  "Силові тренування", 10, 750m,  true,  "Тренер з силових тренувань, майстер спорту з пауерліфтингу."),
            (uByEmail["i.bondarenko@gym.ua"].Id,  "Кардіо",            5,  500m,  false, "Спеціаліст з кардіо тренувань та функціонального фітнесу."),
        };
        var existingTrainerIds = await ctx.Trainers.Select(t => t.UserId).ToHashSetAsync();
        var newTrainers = trainerData
            .Where(d => !existingTrainerIds.Contains(d.Item1))
            .Select(d => new Trainer
            {
                UserId           = d.Item1,
                SpecializationId = specs.First(s => s.Name == d.Item2).Id,
                ExperienceYears  = d.Item3,
                HourlyRate       = d.Item4,
                IsAvailable      = d.Item5,
                Bio              = d.Item6,
            }).ToList();
        if (newTrainers.Count > 0) { ctx.Trainers.AddRange(newTrainers); await ctx.SaveChangesAsync(); }
        var trainers = await ctx.Trainers.ToListAsync();

        // ── 6. Clients ────────────────────────────────────────────────────────
        var clientData = new[]
        {
            (uByEmail["n.petrenko@gym.ua"].Id,  (string?)"Немає протипоказань."),
            (uByEmail["d.melnyk@gym.ua"].Id,    "Проблеми з поперековим відділом хребта — уникати важких присідань."),
            (uByEmail["i.lysenko@gym.ua"].Id,   (string?)null),
            (uByEmail["a.polischuk@gym.ua"].Id, "Алергія на латекс."),
            (uByEmail["t.savchenko@gym.ua"].Id, "Після операції на коліні — обмежені навантаження."),
        };
        var existingClientIds = await ctx.Clients.Select(c => c.UserId).ToHashSetAsync();
        var newClients = clientData
            .Where(d => !existingClientIds.Contains(d.Item1))
            .Select(d => new Client { UserId = d.Item1, MedicalNotes = d.Item2 })
            .ToList();
        if (newClients.Count > 0) { ctx.Clients.AddRange(newClients); await ctx.SaveChangesAsync(); }
        var clientIds = clientData.Select(d => d.Item1).ToArray();
        var clients = await ctx.Clients.Where(c => clientIds.Contains(c.UserId)).ToListAsync();

        // ── 7. Admins ─────────────────────────────────────────────────────────
        var adminUserId = uByEmail["s.kravchenko@gym.ua"].Id;
        if (!await ctx.Admins.AnyAsync(a => a.UserId == adminUserId))
        {
            ctx.Admins.Add(new Admin { UserId = adminUserId, AccessLevel = 1 });
            await ctx.SaveChangesAsync();
        }

        // ── 8. TrainingCategories ─────────────────────────────────────────────
        var catNames = new[] { "Групові", "Індивідуальні", "Онлайн" };
        await UpsertByName(ctx.TrainingCategories, catNames, n => new TrainingCategory { Name = n }, ctx);
        var catDict = await ctx.TrainingCategories.Where(c => catNames.Contains(c.Name))
                               .ToDictionaryAsync(c => c.Name);

        // ── 9. MembershipTypes ────────────────────────────────────────────────
        var mtNames = new[] { "Базовий", "Стандарт", "Преміум" };
        await UpsertByName(ctx.MembershipTypes, mtNames, n => new MembershipType { Name = n }, ctx);
        var mtDict = await ctx.MembershipTypes.Where(t => mtNames.Contains(t.Name))
                              .ToDictionaryAsync(t => t.Name);

        // ── 10. MembershipPlans ───────────────────────────────────────────────
        var planData = new[]
        {
            ("Базовий місячний",     "Базовий",  30,  800m,  "Доступ до залу у будні дні з 7:00 до 18:00."),
            ("Стандарт місячний",    "Стандарт", 30,  1200m, "Повний доступ до залу щодня та групові заняття."),
            ("Стандарт квартальний", "Стандарт", 90,  3200m, "Квартальний абонемент зі знижкою 10%."),
            ("Преміум місячний",     "Преміум",  30,  2000m, "Повний доступ, необмежені персональні тренування та SPA."),
            ("Преміум річний",       "Преміум",  365, 18000m,"Річний преміум абонемент зі знижкою 25%."),
        };
        var existingPlanNames = await ctx.MembershipPlans.Select(p => p.Name).ToHashSetAsync();
        var newPlans = planData
            .Where(d => !existingPlanNames.Contains(d.Item1))
            .Select(d => new MembershipPlan
            {
                Name = d.Item1, TypeId = mtDict[d.Item2].Id,
                DurationDays = d.Item3, Price = d.Item4,
                Description = d.Item5, IsActive = true,
            }).ToList();
        if (newPlans.Count > 0) { ctx.MembershipPlans.AddRange(newPlans); await ctx.SaveChangesAsync(); }
        var planNames = planData.Select(d => d.Item1).ToArray();
        var plans = await ctx.MembershipPlans.Where(p => planNames.Contains(p.Name))
                             .ToDictionaryAsync(p => p.Name);

        // ── 11. UserMemberships ───────────────────────────────────────────────
        var membershipData = new[]
        {
            (clientIds[0], "Стандарт місячний",    today.AddDays(-120), today.AddDays(-91),  "Закінчився",    12),
            (clientIds[0], "Стандарт квартальний", today.AddDays(-90),  today.AddDays(-1),   "Закінчився",    24),
            (clientIds[1], "Преміум місячний",     today.AddDays(-60),  today.AddDays(30),   "Активний",      8 ),
            (clientIds[2], "Базовий місячний",     today.AddDays(-30),  today.AddDays(0),    "Призупинений",  4 ),
            (clientIds[3], "Преміум річний",       today.AddDays(-15),  today.AddDays(350),  "Активний",      3 ),
        };
        foreach (var d in membershipData)
        {
            if (!await ctx.UserMemberships.AnyAsync(m => m.ClientId == d.Item1 && m.StartDate == d.Item3))
            {
                ctx.UserMemberships.Add(new UserMembership
                {
                    ClientId = d.Item1, MembershipPlanId = plans[d.Item2].Id,
                    StartDate = d.Item3, EndDate = d.Item4,
                    StatusId = msDict[d.Item5].Id, SessionsUsed = d.Item6,
                });
            }
        }
        await ctx.SaveChangesAsync();

        // ── 12. TrainingTypes ─────────────────────────────────────────────────
        var ttData = new[]
        {
            ("Хатха йога",          "Групові",        "Класичні йогічні пози для гнучкості та балансу.",              60),
            ("Силове тренування",   "Індивідуальні",  "Персональне тренування з важкою атлетикою.",                   60),
            ("Кардіо HIIT",         "Групові",        "Високоінтенсивне інтервальне тренування для спалення жиру.",    45),
            ("Пілатес",             "Групові",        "Вправи для зміцнення м'язів кору та постави.",                 50),
            ("Онлайн консультація", "Онлайн",         "Індивідуальна онлайн-сесія з тренером.",                       30),
        };
        var existingTtNames = await ctx.TrainingTypes.Select(t => t.Name).ToHashSetAsync();
        var newTt = ttData
            .Where(d => !existingTtNames.Contains(d.Item1))
            .Select(d => new TrainingType
            {
                Name = d.Item1, CategoryId = catDict[d.Item2].Id,
                Description = d.Item3, DefaultDuration = d.Item4,
            }).ToList();
        if (newTt.Count > 0) { ctx.TrainingTypes.AddRange(newTt); await ctx.SaveChangesAsync(); }
        var ttNames = ttData.Select(d => d.Item1).ToArray();
        var tt = await ctx.TrainingTypes.Where(t => ttNames.Contains(t.Name))
                          .ToDictionaryAsync(t => t.Name);

        // ── 13. ScheduledSessions ─────────────────────────────────────────────
        var trainerByEmail = trainers.ToDictionary(t => t.UserId);
        var sessionData = new[]
        {
            ("Хатха йога",          "o.koval@gym.ua",      clientIds[0], today.AddDays(-85), new TimeOnly(9,  0), new TimeOnly(10, 0),  "Завершено",    "Перше знайомче заняття."),
            ("Силове тренування",   "m.shevchenko@gym.ua", clientIds[1], today.AddDays(-70), new TimeOnly(11, 0), new TimeOnly(12, 0),  "Завершено",    "Оцінка фізичної форми."),
            ("Кардіо HIIT",         "i.bondarenko@gym.ua", clientIds[2], today.AddDays(-60), new TimeOnly(18, 0), new TimeOnly(18, 45), "Завершено",    (string?)null),
            ("Хатха йога",          "o.koval@gym.ua",      clientIds[3], today.AddDays(-50), new TimeOnly(10, 0), new TimeOnly(11, 0),  "Скасовано",    "Клієнт не прийшов."),
            ("Силове тренування",   "m.shevchenko@gym.ua", clientIds[0], today.AddDays(-42), new TimeOnly(9,  0), new TimeOnly(10, 0),  "Завершено",    (string?)null),
            ("Пілатес",             "o.koval@gym.ua",      clientIds[1], today.AddDays(-30), new TimeOnly(17, 0), new TimeOnly(17, 50), "Завершено",    "Акцент на зміцнення спини."),
            ("Кардіо HIIT",         "i.bondarenko@gym.ua", clientIds[4], today.AddDays(-20), new TimeOnly(8,  0), new TimeOnly(8,  45), "Завершено",    (string?)null),
            ("Онлайн консультація", "o.koval@gym.ua",      clientIds[2], today.AddDays(-10), new TimeOnly(19, 0), new TimeOnly(19, 30), "Скасовано",    "Технічні проблеми зі зв'язком."),
            ("Силове тренування",   "m.shevchenko@gym.ua", clientIds[3], today.AddDays(2),   new TimeOnly(10, 0), new TimeOnly(11, 0),  "Заплановано",  "Зосередитись на жимі лежачи."),
            ("Хатха йога",          "o.koval@gym.ua",      clientIds[4], today.AddDays(5),   new TimeOnly(9,  0), new TimeOnly(10, 0),  "Заплановано",  (string?)null),
        };
        foreach (var d in sessionData)
        {
            if (!await ctx.ScheduledSessions.AnyAsync(s =>
                    s.ClientId == d.Item3 && s.SessionDate == d.Item4 && s.StartTime == d.Item5))
            {
                ctx.ScheduledSessions.Add(new ScheduledSession
                {
                    TrainingTypeId = tt[d.Item1].Id,
                    TrainerId      = uByEmail[d.Item2].Id,
                    ClientId       = d.Item3,
                    SessionDate    = d.Item4,
                    StartTime      = d.Item5,
                    EndTime        = d.Item6,
                    StatusId       = ssDict[d.Item7].Id,
                    Notes          = d.Item8,
                });
            }
        }
        await ctx.SaveChangesAsync();
    }

    // Insert only names that don't already exist in the table.
    private static async Task UpsertByName<T>(
        Microsoft.EntityFrameworkCore.DbSet<T> dbSet,
        string[] names,
        Func<string, T> factory,
        GymContext ctx) where T : class
    {
        var existing = await dbSet
            .Select(e => EF.Property<string>(e, "Name"))
            .ToHashSetAsync();
        var toAdd = names.Where(n => !existing.Contains(n)).Select(factory).ToList();
        if (toAdd.Count > 0) { dbSet.AddRange(toAdd); await ctx.SaveChangesAsync(); }
    }
}
