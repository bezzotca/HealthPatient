using Avalonia.Media.TextFormatting;
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
    public partial class CheckReviewsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] List<Review> reviews;
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
                    "Фамилия",
                    "Имя",
                    "Отчество",
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
               
            }
            else if (ChangedFilter != "Без фильтра")
            {
                if (value == "" || value == null)
                {
                    
                }
                else if (value != "")
                {
                    switch (ChangedFilter)
                    {
                        case "Фамилия":
                            
                            break;
                        case "Имя":
                           
                            break;
                        case "Отчество":
                            
                            break;
                    }
                }
            }
        }

        partial void OnChangedFilterChanged(string value)
        {
            if (value == "Без фильтра")
            {

            }
            else if (value != "Без фильтра")
            {
                if (TextFind == "" || TextFind == null)
                {

                }
                else if (TextFind != "")
                {
                    switch (value)
                    {
                        case "Фамилия":
                            
                            break;
                        case "Имя":
                            
                            break;
                        case "Отчество":
                            
                            break;
                    }
                }
            }
        }
    }
}
