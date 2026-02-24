using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class MembershipType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
}
