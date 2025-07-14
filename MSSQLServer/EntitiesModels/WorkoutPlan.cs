using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class WorkoutPlan
{
    public int PlanId { get; set; }

    public int AssignmentId { get; set; }

    public byte DayOfWeek { get; set; }

    public string ExerciseName { get; set; } = null!;

    public short? Sets { get; set; }

    public short? Reps { get; set; }

    public string? WeightDescription { get; set; }

    public string? Notes { get; set; }

    public virtual TrainerAssignment Assignment { get; set; } = null!;

    public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}
