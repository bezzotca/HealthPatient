using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class AdminRightsViewModel: ViewModelBase
    {
        [ObservableProperty] string helloMessage;
        [ObservableProperty] string mrMrs;
        [ObservableProperty] Administrator admin;
        public AdminRightsViewModel()
        {
            admin = MainWindowViewModel.Instance.Admin;
            GenerateGreeting();
            MrsOrMs();
        }

        public void GenerateGreeting()
        {
            int hour = DateTime.Now.Hour;
            string timeOfDay = hour switch
            {
                >= 9 and <= 11 => "Утро",
                >= 11 and < 18 => "День",
                _ => "Вечер"
            };

            HelloMessage = $"Добрый {timeOfDay}!";
        }

        public void MrsOrMs()
        {
            if (admin.GenderId== 1)
            {
                MrMrs = "Мистер";
            }
            else
            {
                MrMrs = "Миссис";
            }
        }

        public void Redact()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new RedactInfoAboutAdminViewModel();
        }
        public void AddUser()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new RedactInfoAboutAdminViewModel();
        }
        public void AddNews()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new AddNewViewModel();
        }
    }
}
