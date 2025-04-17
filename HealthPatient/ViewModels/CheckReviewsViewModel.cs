using Avalonia.Media.TextFormatting;
using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class CheckReviewsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<Review> reviews;
        [ObservableProperty] List<Review> reviews0;
        [ObservableProperty] bool isVisibleButton;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string changedFilter;
        [ObservableProperty] string textFind;
        public CheckReviewsViewModel()
        {
            doctor = MainWindowViewModel.Instance.Doctor;
            if(doctor != null)
            {
                reviews = Db.Reviews.Include(x => x.Doctor).Include(x => x.Patient).Where(x => x.DoctorId == doctor.DoctorId).ToList();
                reviews0 = Db.Reviews.Include(x => x.Doctor).Include(x => x.Patient).Where(x => x.DoctorId == doctor.DoctorId).ToList();
            }
            else if(doctor == null && MainWindowViewModel.Instance.Patient == null)
            {
                reviews = Db.Reviews.Include(x => x.Doctor).Include(x => x.Patient).ToList();
            }
            if (MainWindowViewModel.Instance.PrevPage == "DoctorWorkViewModel")
            {
                isVisibleButton = true;
            }
            filter = new List<string>
                {
                    "Без фильтра",
                    "Комментарий",
                };
        }

        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new DoctorWorkViewModel();
        }
        partial void OnTextFindChanged(string value)
        {
            if (ChangedFilter == "Без фильтра")
            {
                Reviews = reviews0;
            }
            else if (ChangedFilter != "Без фильтра")
            {
                if (value == "" || value == null)
                {
                    Reviews = reviews0;
                }
                else if (value != "")
                {
                    switch (ChangedFilter)
                    {
                        case "Комментарий":
                            Reviews = reviews0;
                            Reviews = Db.Reviews.Where(x => x.Comment.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }

        partial void OnChangedFilterChanged(string value)
        {
            if (value == "Без фильтра")
            {
                Reviews = reviews0;
            }
            else if (value != "Без фильтра")
            {
                if (TextFind == "" || TextFind == null)
                {
                    Reviews = reviews0;
                }
                else if (TextFind != "")
                {
                    switch (value)
                    {
                        case "Комментарий":
                            Reviews = reviews0;
                            Reviews = Db.Reviews.Where(x => x.Comment.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }
    }
}
