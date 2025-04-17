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
    public partial class CheckLoyaltyViewModel:ViewModelBase
    {
        [ObservableProperty] Patient patient;
        [ObservableProperty] List<LoyaltyPointsHistory> loyalty;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string changedFilter;
        [ObservableProperty] string textFind;

        public CheckLoyaltyViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
            if(patient != null)
            {
                loyalty = Db.LoyaltyPointsHistories.Include(x => x.Visit).Include(x => x.Patient).Where(x => x.PatientId == patient.PatientId).ToList();
            }
            else if(patient == null && MainWindowViewModel.Instance.Doctor == null)
            {
                loyalty = Db.LoyaltyPointsHistories.Include(x => x.Visit).Include(x => x.Patient).ToList();
            }
            filter = new List<string>
            {
                "Без фильтра",
                "Фамилия",
                "Имя",
                "Отчество",
            };
        }

        public void Save(LoyaltyPointsHistory loyalty)
        {
            Db.LoyaltyPointsHistories.Update(loyalty);
            Db.SaveChanges();
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
