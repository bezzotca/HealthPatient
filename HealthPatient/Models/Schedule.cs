using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? DatestartSchedule { get; set; }

    public bool IsBusy { get; set; }

    public int? Lengthinmins { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
