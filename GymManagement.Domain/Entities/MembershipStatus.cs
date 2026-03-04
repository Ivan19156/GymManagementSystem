namespace GymManagement.Domain.Entities;

public class MembershipStatus : Entity
{
    public string Name { get; set; } = null!;

    public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}