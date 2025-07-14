using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class UserMembership
{
    public int MembershipId { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? AutoRenew { get; set; }

    public virtual MembershipPlan Plan { get; set; } = null!;

    public virtual ICollection<TrainerAssignment> TrainerAssignments { get; set; } = new List<TrainerAssignment>();

    public virtual User User { get; set; } = null!;
}
