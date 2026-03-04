namespace GymManagement.Domain.Entities;

public class TrainingType : Entity, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public int? CategoryId { get; set; }
    public string? Description { get; set; }
    public int? DefaultDuration { get; set; }

    public TrainingCategory? Category { get; set; }
    public ICollection<ScheduledSession> ScheduledSessions { get; set; } = new List<ScheduledSession>();
}