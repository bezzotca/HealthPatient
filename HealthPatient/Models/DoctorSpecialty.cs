using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class DoctorSpecialty
{
    public int? DoctorId { get; set; }

    public int? SpecialtyId { get; set; }

    public string? Qualification { get; set; }

    public DateOnly? CertificationDate { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Specialty? Specialty { get; set; }
}
