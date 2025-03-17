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
    public partial class CheckScheduleViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<Schedule> schedule;

        public CheckScheduleViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
            schedule = Db.Schedules.Include(x=>x.Doctor).Where(x => x.DoctorId == doctor.DoctorId).ToList();
        }
    }
}
