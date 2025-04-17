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
    public partial class DoctorWorkViewModel: ViewModelBase
    {
        [ObservableProperty] List<Schedule> schedule;
        [ObservableProperty] List<News> news;

        public DoctorWorkViewModel()
        {
            schedule = Db.Schedules.Include(x => x.Doctor).ToList();
            news = Db.News.ToList();
        }

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
    }
}
