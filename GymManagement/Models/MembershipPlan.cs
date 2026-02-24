using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class MembershipPlan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? TypeId { get; set; }

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual MembershipType? Type { get; set; }

    public virtual ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}
