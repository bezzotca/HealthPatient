using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class DoctorAchievement
{
    public int? DoctorId { get; set; }

    public int? AchievementId { get; set; }

    public DateOnly? DateAchieved { get; set; }

    public virtual Achievement? Achievement { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
