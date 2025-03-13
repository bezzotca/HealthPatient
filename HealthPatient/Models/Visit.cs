using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public int? ScheduleId { get; set; }

    public DateTime? VisitDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? Prescriptions { get; set; }

    public decimal? Cost { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<LoyaltyPointsHistory> LoyaltyPointsHistories { get; set; } = new List<LoyaltyPointsHistory>();

    public virtual Patient? Patient { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
