using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class MembershipPlan
{
    public int PlanId { get; set; }

    public string Name { get; set; } = null!;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
}
