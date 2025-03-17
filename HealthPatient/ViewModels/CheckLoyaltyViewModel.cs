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

        public CheckLoyaltyViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
            loyalty = Db.LoyaltyPointsHistories.Include(x=>x.Visit).Include(x=>x.Patient).Where(x=>x.PatientId == patient.PatientId).ToList();
        }

        public void Save(LoyaltyPointsHistory loyalty)
        {
            Db.LoyaltyPointsHistories.Update(loyalty);
            Db.SaveChanges();
        }
    }
}
