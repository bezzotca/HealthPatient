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
        [ObservableProperty] List<LoyaltyPointsHistory> loyalty0;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string changedFilter;
        [ObservableProperty] string textFind;

        public CheckLoyaltyViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
            if(patient != null)
            {
                loyalty = Db.LoyaltyPointsHistories.Include(x => x.Visit).Include(x => x.Patient).Where(x => x.PatientId == patient.PatientId).ToList();
                loyalty0 = Db.LoyaltyPointsHistories.Include(x => x.Visit).Include(x => x.Patient).Where(x => x.PatientId == patient.PatientId).ToList();
            }
            else if(patient == null && MainWindowViewModel.Instance.Doctor == null)
            {
                loyalty = Db.LoyaltyPointsHistories.Include(x => x.Visit).Include(x => x.Patient).ToList();
            }
            filter = new List<string>
            {
                "Без фильтра",
                "Причина",
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
                Loyalty = loyalty0;
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
                        case "Причина":
                            Loyalty = loyalty0;
                            Loyalty = Db.LoyaltyPointsHistories.Where(x=>x.Reason.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }

        partial void OnChangedFilterChanged(string value)
        {
            if (ChangedFilter == "Без фильтра")
            {
                Loyalty = loyalty0;
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
                        case "Причина":
                            Loyalty = loyalty0;
                            Loyalty = Db.LoyaltyPointsHistories.Where(x => x.Reason.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }
    }
}
