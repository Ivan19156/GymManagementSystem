using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class UserMembership
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int MembershipPlanId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int StatusId { get; set; }

    public int? SessionsUsed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual MembershipPlan MembershipPlan { get; set; } = null!;

    public virtual MembershipStatus Status { get; set; } = null!;
}
