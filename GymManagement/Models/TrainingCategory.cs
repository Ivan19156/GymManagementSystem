using System;
using System.Collections.Generic;

namespace GymManagement.Models;

public partial class TrainingCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TrainingType> TrainingTypes { get; set; } = new List<TrainingType>();
}
