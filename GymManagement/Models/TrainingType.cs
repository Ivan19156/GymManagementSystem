using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class TrainingType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public int? DefaultDuration { get; set; }

    public virtual TrainingCategory? Category { get; set; }

    public virtual ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
}
