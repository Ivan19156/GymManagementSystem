namespace GymManagement.Domain.Entities;

public class Specialization : Entity, IAggregateRoot
{
    public string Name { get; set; } = null!;

    public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}