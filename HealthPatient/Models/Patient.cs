using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? ContactPhone { get; set; }

    public string? Email { get; set; }

    public string? MedicalNotes { get; set; }

    public int? LoyaltyPoints { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? GenderId { get; set; }

    public string? Image { get; set; }

    public Bitmap Photo => ConverterToBitmapImage.ConvertToPatient(Image, GenderId);
    public virtual Gender? Gender { get; set; }

    public virtual ICollection<LoyaltyPointsHistory> LoyaltyPointsHistories { get; set; } = new List<LoyaltyPointsHistory>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
