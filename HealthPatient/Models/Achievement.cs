using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Achievement
{
    public int AchievementId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Criteria { get; set; }

    public string? BadgeImage { get; set; }
}
