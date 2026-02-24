using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class MembershipStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}
