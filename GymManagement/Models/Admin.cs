using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class Admin
{
    public int UserId { get; set; }

    public int? AccessLevel { get; set; }

    public virtual User User { get; set; } = null!;
}
