using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class RedactInfoAboutPatientViewModel: ViewModelBase
    {
        [ObservableProperty] Patient patient;

        public RedactInfoAboutPatientViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
        }

        public void Save(Patient patient)
        {
            patient.UpdatedAt = DateTime.Now;   
            Db.Patients.Update(patient);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new PatientsMainViewModel();
        }
        public void Delete (Patient patient)
        {
            Db.Patients.Remove(patient);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new PatientsMainViewModel();
        }
    }
}
