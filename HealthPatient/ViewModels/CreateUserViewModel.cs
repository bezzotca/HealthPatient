using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class CreateUserViewModel: ViewModelBase
    {
        [ObservableProperty] bool isVisible;
        [ObservableProperty] bool isVisiblePatient;
        [ObservableProperty] bool isVisibleDoctor;
        [ObservableProperty] bool isVisibleBorder;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string changedFilter;
        [ObservableProperty] string firstName;
        [ObservableProperty] string lastName;
        [ObservableProperty] string patronymic;
        [ObservableProperty] string bio;
        [ObservableProperty] string login;
        [ObservableProperty] string password;
        [ObservableProperty] string contactPhone;
        [ObservableProperty] string email;
        [ObservableProperty] string notes;
        [ObservableProperty] List<Gender> genders;
        [ObservableProperty] Gender selectedGender;
        public CreateUserViewModel()
        {
            filter = new List<string>
            {
                "Врач",
                "Пациент"
            };
           genders = Db.Genders.ToList();
        }

        partial void OnChangedFilterChanged(string value)
        {
            IsVisible = false;
            IsVisiblePatient = false;
            IsVisibleBorder = false;
            switch (value)
            {
                case "Врач":
                    IsVisible = true;
                    IsVisibleDoctor = true;
                    IsVisibleBorder = true;
                    break;
                case "Пациент":
                    IsVisible = true;
                    IsVisiblePatient = true;
                    IsVisibleBorder = true;
                    break;
                default:
                    IsVisible = false;
                    IsVisiblePatient = false;
                    IsVisibleBorder = false;
                    break;
            }
        }

        public void CreateUser()
        {
            switch(ChangedFilter)
            {
                case "Врач":
                    if(FirstName != null && FirstName != "" && LastName != null 
                        && LastName != "" && Patronymic != null && Patronymic != "" && Bio != null && Bio != ""
                        && Login != null && Login != "" && Password != null && Password != "")
                    {
                        Doctor doctor = new Doctor()
                        {
                            DoctorId = Db.Doctors.Select(x=>x.DoctorId).Max() + 1,
                            FirstName = FirstName,
                            LastName = LastName,
                            Patronymic = Patronymic,
                            Bio = Bio,
                            Login = Login,
                            Password = Password,
                            Image = "None",
                            GenderId = SelectedGender.GenderId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                        Db.Doctors.Add(doctor);
                        Db.SaveChanges();
                    }
                    break;
                case "Пациент":
                    if (FirstName != null && FirstName != "" && LastName != null
                        && LastName != "" && Patronymic != null && Patronymic != ""
                        && Login != null && Login != "" && Password != null && Password != ""
                        && Email != null && Email != "" && ContactPhone != null && ContactPhone != "")
                    {
                        Patient patient = new Patient()
                        {
                            PatientId = Db.Patients.Select(x => x.PatientId).Max() + 1,
                            FirstName = FirstName,
                            LastName = LastName,
                            Patronymic = Patronymic,
                            ContactPhone = ContactPhone,
                            Email = Email,
                            Login = Login,
                            Password = Password,
                            Image = "None",
                            GenderId = SelectedGender.GenderId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                        Db.Patients.Add(patient);
                        Db.SaveChanges();
                    }
                    break;
            }
        }
    }
}
