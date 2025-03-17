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
    public partial class PatientsMainViewModel : ViewModelBase
    {
        [ObservableProperty] List<Patient> patients;
        [ObservableProperty] List<Patient> patients0;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string textFind;
        [ObservableProperty] string changedFilter;
        public PatientsMainViewModel()
        {
            patients = Db.Patients.Include(x=>x.LoyaltyPointsHistories).Include(x=>x.Notifications).Include(x=>x.Reviews).Include(x=>x.Visits).ToList();
            patients0 = Db.Patients.Include(x=>x.LoyaltyPointsHistories).Include(x=>x.Notifications).Include(x=>x.Reviews).Include(x=>x.Visits).ToList();
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
                Patients = patients0;
            }
            else if (ChangedFilter != "Без фильтра")
            {
                if (value == "" || value == null)
                {
                    Patients = patients0;
                }
                else if (value != "")
                {
                    switch (ChangedFilter)
                    {
                        case "Фамилия":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.LastName.Contains(TextFind)).ToList();
                            break;
                        case "Имя":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.FirstName.Contains(TextFind)).ToList();
                            break;
                        case "Отчество":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.Patronymic.Contains(TextFind)).ToList();
                            break;
                    }
                }
            }
        }

        partial void OnChangedFilterChanged(string value)
        {
            if (value == "Без фильтра")
            {
                Patients = patients0;
            }
            else if (value != "Без фильтра")
            {
                if (TextFind == "" || TextFind == null)
                {
                    Patients = patients0;
                }
                else if (TextFind != "")
                {
                    switch (value)
                    {
                        case "Фамилия":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.LastName.Contains(TextFind)).ToList();
                            break;
                        case "Имя":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.FirstName.Contains(TextFind)).ToList();
                            break;
                        case "Отчество":
                            Patients = patients0;
                            Patients = Patients.Where(x => x.Patronymic.Contains(TextFind)).ToList();
                            break;
                    }
                }
            }
        }
    }
}
