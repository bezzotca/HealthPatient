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
    public partial class NotificationsViewModel:ViewModelBase
    {
        [ObservableProperty] List<Notification> notifications;
        [ObservableProperty] Patient patient;
        [ObservableProperty] bool isVisibleButton = false;

        public NotificationsViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
            notifications = Db.Notifications.Include(x=>x.Doctor).Include(x=>x.Patient).Where(x=>x.PatientId == patient.PatientId).ToList();
            if(MainWindowViewModel.Instance.PrevPage == "PatientWorkViewModel")
            {
                isVisibleButton = true;
            }
        }

        public void Save(Notification not)
        {
            Db.Notifications.Update(not);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new NotificationsViewModel();
        }
        public void Read(Notification not)
        {
            not.Status = "Прочитано";
            Db.Notifications.Update(not);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new NotificationsViewModel();
        }
        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new PatientWorkViewModel();
        }
    }
}
