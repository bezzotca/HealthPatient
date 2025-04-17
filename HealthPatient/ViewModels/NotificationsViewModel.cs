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
        [ObservableProperty] bool isVisibleButton2 = false;

        public NotificationsViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
            notifications = Db.Notifications.Include(x=>x.Doctor).Include(x=>x.Patient).Where(x=>x.PatientId == patient.PatientId).ToList();
            if (MainWindowViewModel.Instance.PrevPage == "PatientWorkViewModel")
            {
                isVisibleButton = true;
                isVisibleButton2 = false;
            }
            else
            {
                isVisibleButton2 = true;
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
            if (MainWindowViewModel.Instance.PrevPage == "DoctorsMainViewModel" || MainWindowViewModel.Instance.PrevPage == "PatientsMainViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new AdminMainViewModel();
                MainWindowViewModel.Instance.PageSwitcherAdminPanel = new NotificationsViewModel();
            }
            else
            {
                MainWindowViewModel.Instance.PageSwitcher = new NotificationsViewModel();
            }
        }
        public void UnRead(Notification not)
        {
            not.Status = "Не прочитано";
            Db.Notifications.Update(not);
            Db.SaveChanges();
            if(MainWindowViewModel.Instance.PrevPage == "DoctorsMainViewModel" || MainWindowViewModel.Instance.PrevPage == "PatientsMainViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new AdminMainViewModel();
                MainWindowViewModel.Instance.PageSwitcherAdminPanel = new NotificationsViewModel();
            }
            else
            {
                MainWindowViewModel.Instance.PageSwitcher = new NotificationsViewModel();
            }
        }
        public void GoBack()
        {
            if (MainWindowViewModel.Instance.PrevPage == "DoctorWorkViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new DoctorWorkViewModel();
            }
            else if (MainWindowViewModel.Instance.PrevPage == "PatientWorkViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new PatientWorkViewModel();
            }
        }
    }
}
