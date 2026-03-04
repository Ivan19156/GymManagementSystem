namespace GymManagement.Domain.Entities;

public class SessionStatus : Entity
{
    public string Name { get; set; } = null!;

    public ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
}