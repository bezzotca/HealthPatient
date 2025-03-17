using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class RedactInfoAboutAdminViewModel: ViewModelBase
    {
        [ObservableProperty] Administrator admin;

        public RedactInfoAboutAdminViewModel()
        {
            admin = MainWindowViewModel.Instance.Admin;
        }

        public void Save(Administrator admin)
        {
            admin.UpdatedAt = DateTime.Now;
            Db.Administrators.Update(admin);
            Db.SaveChanges();
        }

        public void Delete(Administrator admin)
        {
            Db.Administrators.Remove(admin);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcher = new AdminMainViewModel();
        }
    }
}
