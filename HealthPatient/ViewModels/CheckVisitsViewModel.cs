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
    public partial class CheckVisitsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor = MainWindowViewModel.Instance.Doctor;
        [ObservableProperty] List<Visit> visits;
        public CheckVisitsViewModel()
        {
            visits = Db.Visits.Include(x=>x.Patient).Include(x=>x.Schedule).Include(x => x.Doctor).Where(x=>x.VisitId == doctor.DoctorId).ToList();
        }

        public void Save(Visit visit)
        {
            if(visit.Patient != null)
            {
                visit.Patient.UpdatedAt = DateTime.Now;
            }

            if (visit.Doctor != null)
            {
                visit.Doctor.UpdatedAt = DateTime.Now;
            }
            visit.UpdatedAt = DateTime.Now;
            Db.SaveChanges();
        }
    }
}
