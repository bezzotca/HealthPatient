using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class AchievementsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<DoctorAchievement> achievements;
        [ObservableProperty] int count;
        [ObservableProperty] int allCount;

        public AchievementsViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
            achievements = Db.DoctorAchievements.Include(x=>x.Doctor).Include(x=>x.Achievement).Where(x=>x.DoctorId == doctor.DoctorId).ToList();
            count = achievements.Count;
            allCount = Db.Achievements.Count();
        }
    }
}
