using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class AnalyzedDatum
{
    public int IdData { get; set; }

    public int IdDoctor { get; set; }

    public decimal? MoneyCounted { get; set; }

    public int? PatientsCounted { get; set; }

    public int? HoursInWork { get; set; }

    public decimal? Averagerating { get; set; }

    public int? Countbadrating { get; set; }

    public int? Countgoodrating { get; set; }

    public virtual Doctor IdDoctorNavigation { get; set; } = null!;
}
