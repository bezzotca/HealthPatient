using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int? DoctorId { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
