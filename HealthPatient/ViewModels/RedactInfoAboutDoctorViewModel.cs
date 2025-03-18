using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class RedactInfoAboutDoctorViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;

        public RedactInfoAboutDoctorViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
        }
        
        public void Save(Doctor doctor)
        {
            doctor.UpdatedAt = DateTime.Now;
            Db.Doctors.Update(doctor);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new DoctorsMainViewModel();
        }

        public void Delete(Doctor doctor)
        {
            Db.Doctors.Remove(doctor);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new DoctorsMainViewModel();
        }
    }
}
