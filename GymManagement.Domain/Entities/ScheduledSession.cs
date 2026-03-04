namespace GymManagement.Domain.Entities;

public class ScheduledSession : Entity, IAggregateRoot
{
    public int TrainingTypeId { get; set; }
    public int? TrainerId { get; set; }
    public int ClientId { get; set; }
    public DateOnly SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int StatusId { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }

    public Client Client { get; set; } = null!;
    public Trainer? Trainer { get; set; }
    public TrainingType TrainingType { get; set; } = null!;
    public SessionStatus Status { get; set; } = null!;
}