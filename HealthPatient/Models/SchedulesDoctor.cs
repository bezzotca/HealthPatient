using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class SchedulesDoctor
{
    public int DocSchedId { get; set; }

    public int? DoctorId { get; set; }

    public int? ScheduleId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
