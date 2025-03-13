using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class SymptomSpecialty
{
    public int? SymptomId { get; set; }

    public int? SpecialtyId { get; set; }

    public short? Priority { get; set; }

    public virtual Specialty? Specialty { get; set; }

    public virtual Symptom? Symptom { get; set; }
}
