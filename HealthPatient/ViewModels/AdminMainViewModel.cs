using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class AdminMainViewModel: ViewModelBase
    {
        [ObservableProperty] ViewModelBase pageSwitcher;
        public static AdminMainViewModel Instance { get; set; }

        public AdminMainViewModel()
        {
            Instance = this;
        }
        public void CheckDoctors()
        {
            
            PageSwitcher = new DoctorsMainViewModel();
        }
    }
}
