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
            if(MainWindowViewModel.Instance.PageSwitcherAdminPanel != null)
            {
                if(MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CheckVisitsViewModel")
                {
                    pageSwitcher = new CheckVisitsViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "AchievementsViewModel")
                {
                    pageSwitcher = new AchievementsViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CheckReviewsViewModel")
                {
                    pageSwitcher = new CheckReviewsViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CheckScheduleViewModel")
                {
                    pageSwitcher = new CheckScheduleViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CheckSpecialtyViewModel")
                {
                    pageSwitcher = new CheckSpecialtyViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "RedactInfoAboutDoctorViewModel")
                {
                    pageSwitcher = new RedactInfoAboutDoctorViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CheckLoyaltyViewModel")
                {
                    pageSwitcher = new CheckLoyaltyViewModel();
                }
            }
                
        }
        public void CheckDoctors()
        {   
            AdminMainViewModel.Instance.PageSwitcher = new DoctorsMainViewModel();
        }
        
        public void CheckPatients()
        {
            AdminMainViewModel.Instance.PageSwitcher = new PatientsMainViewModel();
        }
    }
}
