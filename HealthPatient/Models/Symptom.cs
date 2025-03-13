using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Symptom
{
    public int SymptomId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
