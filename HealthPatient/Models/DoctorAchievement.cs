using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace HealthPatient.Models;

public partial class DoctorAchievement
{
    public int? DoctorId { get; set; }

    public int? AchievementId { get; set; }

    public DateOnly? DateAchieved { get; set; }

    public Bitmap Image => ConverterToBitmapImage.ConvertToAchieve(Achievement.BadgeImage,AchievementId);

    public virtual Achievement? Achievement { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
