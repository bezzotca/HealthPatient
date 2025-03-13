using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Specialty
{
    public int SpecialtyId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
