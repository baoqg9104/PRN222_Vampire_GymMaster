using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class WorkoutSession
{
    public int SessionId { get; set; }

    public int MemberId { get; set; }

    public int PlanId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public short? ActualSets { get; set; }

    public short? ActualReps { get; set; }

    public string? ActualWeight { get; set; }

    public string? Notes { get; set; }

    public byte? Rating { get; set; }

    public virtual User Member { get; set; } = null!;

    public virtual WorkoutPlan Plan { get; set; } = null!;
}
