using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class PatientWorkViewModel: ViewModelBase
    {

        public void CheckVisits()
        {
            MainWindowViewModel.Instance.PrevPage = MainWindowViewModel.Instance.PageSwitcher.GetType().Name;
            MainWindowViewModel.Instance.PageSwitcher = new CheckVisitsViewModel();
        }
        public void CheckReviews()
        {
            MainWindowViewModel.Instance.PrevPage = MainWindowViewModel.Instance.PageSwitcher.GetType().Name;
            MainWindowViewModel.Instance.PageSwitcher = new CheckReviewsViewModel();
        }
        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new LoginViewModel();
        }
        public void GoToNotifications()
        {
            MainWindowViewModel.Instance.PrevPage = MainWindowViewModel.Instance.PageSwitcher.GetType().Name;
            MainWindowViewModel.Instance.PageSwitcher = new NotificationsViewModel();
        }
        public void CreateVisit()
        {
            MainWindowViewModel.Instance.PrevPage = MainWindowViewModel.Instance.PageSwitcher.GetType().Name;
            MainWindowViewModel.Instance.PageSwitcher = new AddVisitsViewModel();
        }
    }
}
