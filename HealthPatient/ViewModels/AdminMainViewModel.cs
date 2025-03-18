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
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "RedactInfoAboutPatientViewModel")
                {
                    pageSwitcher = new RedactInfoAboutPatientViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "NotificationsViewModel")
                {
                    pageSwitcher = new NotificationsViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "AddNewViewModel")
                {
                    pageSwitcher = new AddNewViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "RedactInfoAboutAdminViewModel")
                {
                    pageSwitcher = new RedactInfoAboutAdminViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "CreateUserViewModel")
                {
                    pageSwitcher = new CreateUserViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "DoctorsMainViewModel")
                {
                    pageSwitcher = new DoctorsMainViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "PatientsMainViewModel")
                {
                    pageSwitcher = new PatientsMainViewModel();
                }
                else if (MainWindowViewModel.Instance.PageSwitcherAdminPanel.GetType().Name == "AdminRightsViewModel")
                {
                    pageSwitcher = new AdminRightsViewModel();
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
        public void CheckAllVisits()
        {
            MainWindowViewModel.Instance.Doctor = null;
            MainWindowViewModel.Instance.Patient = null;
            AdminMainViewModel.Instance.PageSwitcher = new CheckVisitsViewModel();
        }
        public void CheckAllReviews()
        {
            MainWindowViewModel.Instance.Doctor = null;
            MainWindowViewModel.Instance.Patient = null;
            AdminMainViewModel.Instance.PageSwitcher = new CheckReviewsViewModel();
        }
        public void CheckAllLoyalty()
        {
            MainWindowViewModel.Instance.Doctor = null;
            MainWindowViewModel.Instance.Patient = null;
            AdminMainViewModel.Instance.PageSwitcher = new CheckLoyaltyViewModel();
        }
        public void CheckAdminRights()
        {
            MainWindowViewModel.Instance.Doctor = null;
            MainWindowViewModel.Instance.Patient = null;
            AdminMainViewModel.Instance.PageSwitcher = new AdminRightsViewModel();
        }

        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new LoginViewModel();
        }
    }
}
