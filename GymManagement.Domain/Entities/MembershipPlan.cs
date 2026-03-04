namespace GymManagement.Domain.Entities;

public class MembershipPlan : Entity, IAggregateRoot
{
    public string Name { get; set; } = null!;
    public int? TypeId { get; set; }
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }

    public MembershipType? Type { get; set; }
    public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}