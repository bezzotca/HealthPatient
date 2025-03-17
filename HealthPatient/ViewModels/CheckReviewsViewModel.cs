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
    public partial class CheckReviewsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<Review> reviews;
        public CheckReviewsViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
            reviews = Db.Reviews.Include(x=>x.Doctor).Include(x=>x.Patient).Where(x=>x.DoctorId == doctor.DoctorId).ToList();
        }
    }
}
