using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class LoyaltyPointsHistory
{
    public int LoyaltyHistoryId { get; set; }

    public int? PatientId { get; set; }

    public int? PointsAdded { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Patient? Patient { get; set; }
}
