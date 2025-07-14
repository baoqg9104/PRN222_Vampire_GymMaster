using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class TrainerAssignment
{
    public int AssignmentId { get; set; }

    public int MemberId { get; set; }

    public int TrainerId { get; set; }

    public int MembershipId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Goals { get; set; }

    public bool? IsActive { get; set; }

    public virtual User Member { get; set; } = null!;

    public virtual UserMembership Membership { get; set; } = null!;

    public virtual User Trainer { get; set; } = null!;

    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
}
