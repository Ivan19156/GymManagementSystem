namespace GymManagement.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Admin? Admin { get; set; }
    public Client? Client { get; set; }
    public Trainer? Trainer { get; set; }
}