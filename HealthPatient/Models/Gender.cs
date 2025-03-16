using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string? NameGender { get; set; }

    public virtual ICollection<Administrator> Administrators { get; set; } = new List<Administrator>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
