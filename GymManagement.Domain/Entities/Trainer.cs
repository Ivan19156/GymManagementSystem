namespace GymManagement.Domain.Entities;

public class Trainer
{
    public int UserId { get; set; }
    public int? SpecializationId { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Bio { get; set; }
    public decimal? HourlyRate { get; set; }
    public bool? IsAvailable { get; set; }

    public User User { get; set; } = null!;
    public Specialization? Specialization { get; set; }
    public ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
}