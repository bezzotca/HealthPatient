using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public string? Message { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
