using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class AddVisitsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor;
        [ObservableProperty] Schedule schedule;
        [ObservableProperty] ServicePrice servicePrice;
        // Свойство для даты и времени (DateTimeOffset)
        [ObservableProperty] private DateTimeOffset dateTimeOffset;

        // Свойство для времени (TimeSpan)
        [ObservableProperty] private TimeSpan timeSpan = new TimeSpan(14, 30, 45);
        [ObservableProperty] decimal cost;
        [ObservableProperty] List<Doctor> doctors;
        [ObservableProperty] List<ServicePrice> services;
        [ObservableProperty] bool isVisible;
        [ObservableProperty] bool isVisibleServicePrice;

     

        public AddVisitsViewModel()
        {
            dateTimeOffset = DateTimeOffset.Now;
            doctors = Db.Doctors.ToList();
        }
        partial void OnDoctorChanged(Doctor value)
        {
            IsVisible = true;
            isVisibleServicePrice = true;
            Schedule = Db.Schedules.FirstOrDefault(x => x.DoctorId == value.DoctorId);
            Services = Db.ServicePrices.Include(x=>x.Service).Where(x => x.DoctorId == value.DoctorId).ToList();
        }
        public void AddVisit()
        {
            if (Doctor == null || Schedule == null || ServicePrice == null || TimeSpan == null || DateTimeOffset == null)
            {

            }
            else
            {
                DateTime date = new DateTime(
                DateTimeOffset.Year,
                DateTimeOffset.Month,
                DateTimeOffset.Day,
                TimeSpan.Hours,
                TimeSpan.Minutes,
                TimeSpan.Seconds
            );
                Visit visit = new Visit()
                {
                    VisitId = Db.Visits.Select(x => x.VisitId).Max()+1,
                    PatientId = MainWindowViewModel.Instance.Patient.PatientId,
                    DoctorId = Doctor.DoctorId,
                    ScheduleId = Schedule.ScheduleId,
                    VisitDate = date,
                    Cost = ServicePrice.Price,
                    Status = "Назначено",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                Db.Visits.Add(visit);
                Db.SaveChanges();
                MainWindowViewModel.Instance.PageSwitcher = new PatientWorkViewModel();
            }
        }

        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new PatientWorkViewModel();
        }
    }
}
