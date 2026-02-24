using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class SessionStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
}
