using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public short? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
