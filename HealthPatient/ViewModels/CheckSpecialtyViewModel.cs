using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
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

        public CheckSpecialtyViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
        }
    }
}
