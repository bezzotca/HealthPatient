using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class ServicePrice
{
    public int ServicePriceId { get; set; }

    public int? DoctorId { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Service? Service { get; set; }
}
