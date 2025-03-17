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
    public partial class CheckSpecialtyViewModel:ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<SymptomSpecialty> symptomSpecialties;
        [ObservableProperty] List<int?> specList;

        public CheckSpecialtyViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
            specList = Db.DoctorSpecialties.Where(x => x.DoctorId == doctor.DoctorId).Select(x=>x.SpecialtyId).ToList();
            symptomSpecialties = Db.SymptomSpecialties.Include(x=>x.Symptom).Include(x=>x.Specialty).Where(x => specList.Contains(x.SpecialtyId)).ToList();
        }
    }
}
