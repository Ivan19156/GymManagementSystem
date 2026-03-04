namespace GymManagement.Domain.Entities;

public class MembershipType : Entity
{
    public string Name { get; set; } = null!;

    public ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
}