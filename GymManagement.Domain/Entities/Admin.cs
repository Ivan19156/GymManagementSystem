namespace GymManagement.Domain.Entities;

public class Admin
{
    public int UserId { get; set; }
    public int? AccessLevel { get; set; }

    public User User { get; set; } = null!;
}