using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public string? Bio { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? GenderId { get; set; }

    public string? Image { get; set; }

    public virtual Gender? Gender { get; set; }
    public Bitmap Photo => ConverterToBitmapImage.ConvertToDoctor(Image, GenderId);
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
