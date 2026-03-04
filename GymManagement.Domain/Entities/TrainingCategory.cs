namespace GymManagement.Domain.Entities;

public class TrainingCategory : Entity, IAggregateRoot
{
    public string Name { get; set; } = null!;

    public ICollection<TrainingType> TrainingTypes { get; set; } = new List<TrainingType>();
}