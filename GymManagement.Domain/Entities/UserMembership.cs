namespace GymManagement.Domain.Entities;

public class UserMembership : Entity, IAggregateRoot
{
    public int ClientId { get; set; }
    public int MembershipPlanId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int StatusId { get; set; }
    public int? SessionsUsed { get; set; }
    public DateTime? CreatedAt { get; set; }

    public Client Client { get; set; } = null!;
    public MembershipPlan MembershipPlan { get; set; } = null!;
    public MembershipStatus Status { get; set; } = null!;
}