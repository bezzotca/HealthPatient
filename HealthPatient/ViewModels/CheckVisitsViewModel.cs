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
        [ObservableProperty] Patient patient = MainWindowViewModel.Instance.Patient;
        [ObservableProperty] List<Visit> visits;
        public CheckVisitsViewModel()
        {
            if (doctor != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.DoctorId == doctor.DoctorId).ToList();
            }
            else if(patient != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.PatientId == patient.PatientId).ToList();
            }
            else if(doctor == null && patient == null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).ToList();
            }
            
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
            Db.Visits.Update(visit);
            Db.SaveChanges();
        }
    }
}
