using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class Trainer
{
    public int UserId { get; set; }

    public int? SpecializationId { get; set; }

    public int? ExperienceYears { get; set; }

    public string? Bio { get; set; }

    public decimal? HourlyRate { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();

    public virtual Specialization? Specialization { get; set; }

    public virtual User User { get; set; } = null!;
}
