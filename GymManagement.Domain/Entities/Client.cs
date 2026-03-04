namespace GymManagement.Domain.Entities;

public class Client
{
    public int UserId { get; set; }
    public string? MedicalNotes { get; set; }

    public User User { get; set; } = null!;
    public ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
    public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}