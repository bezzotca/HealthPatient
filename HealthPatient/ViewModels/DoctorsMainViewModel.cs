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
    public partial class DoctorsMainViewModel : ViewModelBase
    {
        [ObservableProperty] List<Doctor> doctors;
        [ObservableProperty] List<Doctor> doctors0;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string textFind;
        [ObservableProperty] string changedFilter;

        public DoctorsMainViewModel()
        {
            doctors = Db.Doctors.Include(x => x.Notifications).Include(x => x.Reviews).Include(x => x.Schedules).Include(x => x.ServicePrices).Include(x => x.Visits).ToList();
            doctors0 = Db.Doctors.Include(x => x.Notifications).Include(x => x.Reviews).Include(x => x.Schedules).Include(x => x.ServicePrices).Include(x => x.Visits).ToList();
            filter = new List<string>
            {
                "Без фильтра",
                "Фамилия",
                "Имя",
                "Отчество",
            };
        }

        partial void OnTextFindChanged(string value)
        {
            if (ChangedFilter == "Без фильтра")
            {
                Doctors = doctors0;
            }
            else if (ChangedFilter != "Без фильтра")
            {
                if (value == "" || value == null)
                {
                    Doctors = doctors0;
                }
                else if (value != "")
                {
                    switch (ChangedFilter)
                    {
                        case "Фамилия":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x => x.LastName.Contains(TextFind)).ToList();
                            break;
                        case "Имя":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x => x.FirstName.Contains(TextFind)).ToList();
                            break;
                        case "Отчество":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x => x.Patronymic.Contains(TextFind)).ToList();
                            break;
                    }
                }
            }
        }

        partial void OnChangedFilterChanged(string value)
        {
            if(value == "Без фильтра")
            {
                Doctors = doctors0;
            }    
            else if (value != "Без фильтра")
            {
                if(TextFind == "" || TextFind == null)
                {
                    Doctors = doctors0;
                }
                else if( TextFind != "")
                {
                    switch (value)
                    {
                        case "Фамилия":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x=>x.LastName.Contains(TextFind)).ToList();
                            break;
                        case "Имя":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x=>x.FirstName.Contains(TextFind)).ToList();
                            break;
                        case "Отчество":
                            Doctors = doctors0;
                            Doctors = Doctors.Where(x=>x.Patronymic.Contains(TextFind)).ToList();
                            break;
                    }
                }
            }
        }

        public void CheckVisits(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new CheckVisitsViewModel();
        }

        public void RedactInfoAboutDoctor(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
        }

        public void CheckAchievements(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new AchievementsViewModel();
        }
        
        public void CheckSpecialty(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
        }
        
        public void CheckReviews(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new CheckReviewsViewModel();
        }

        public void CheckSchedule(Doctor doctor)
        {
            MainWindowViewModel.Instance.Doctor = doctor;
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new CheckScheduleViewModel();
        }


    }
}
