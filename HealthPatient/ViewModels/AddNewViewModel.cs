using Avalonia.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class AddNewViewModel: ViewModelBase
    {
        [ObservableProperty] string title;
        [ObservableProperty] string mainText;

        public void AddNew()
        {
            if (Title != null && MainText != null && Title != "" && MainText != "")
            {
                News new1 = new News()
                {
                    Newid = Db.News.Select(x => x.Newid).Max() + 1,
                    Titlenew = Title,
                    Maintext = MainText,
                };
                Db.News.Add(new1);
                Db.SaveChanges();
            }            
        }
    }
}
