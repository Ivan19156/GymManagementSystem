using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class Specialization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}
