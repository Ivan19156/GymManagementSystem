using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class ScheduledSession
{
    public int Id { get; set; }

    public int TrainingTypeId { get; set; }

    public int? TrainerId { get; set; }

    public int ClientId { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int StatusId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual SessionStatus Status { get; set; } = null!;

    public virtual Trainer? Trainer { get; set; }

    public virtual TrainingType TrainingType { get; set; } = null!;
}
