using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class Client
{
    public int UserId { get; set; }

    public string? MedicalNotes { get; set; }

    public virtual ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}
