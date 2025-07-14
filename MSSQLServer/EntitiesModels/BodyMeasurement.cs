using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class BodyMeasurement
{
    public int MeasurementId { get; set; }

    public int MemberId { get; set; }

    public DateTime? MeasuredAt { get; set; }

    public decimal? WeightKg { get; set; }

    public decimal? BodyFatPct { get; set; }

    public decimal? MuscleMassKg { get; set; }

    public string? Notes { get; set; }

    public virtual User Member { get; set; } = null!;
}
