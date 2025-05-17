using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using System.Linq;

namespace HealthPatient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] ViewModelBase pageSwitcher;
        [ObservableProperty] ViewModelBase pageSwitcherAdminPanel;
        [ObservableProperty] public Administrator admin;
        [ObservableProperty] public Patient patient;
        [ObservableProperty] public Doctor doctor;
        [ObservableProperty] string prevPage;

        public static MainWindowViewModel Instance { get; set; }
        public MainWindowViewModel() 
        {
            Instance = this;
            PageSwitcher = new SplashScreenViewModel();
            Calculate();
        }

        partial void OnPageSwitcherAdminPanelChanged(ViewModelBase value)
        {
            PageSwitcher = new AdminMainViewModel();
        }

        public void Calculate()
        {
            var doctors = Db.Doctors.ToList();

            foreach (var doctor in doctors)
            {
                if (doctor.IsAnalyzed)
                    continue;

                var doctorId = doctor.DoctorId;

                // Подсчет общей выручки
                decimal? countmoney = Db.Visits
                    .Where(x => x.DoctorId == doctor.DoctorId)
                    .Select(x => x.Cost)
                    .Sum();

                // Подсчет пациентов
                int countperson = Db.Visits
                    .Count(x => x.DoctorId == doctor.DoctorId);

                // Подсчет рабочих часов
                int? hrs = Db.Schedules
                    .Where(s => s.DoctorId == doctor.DoctorId && s.IsBusy == true)
                    .Sum(s => (int?)s.Lengthinmins);

                // Работа с отзывами
                var reviews = Db.Reviews
                    .Where(x => x.DoctorId == doctor.DoctorId )
                    .ToList();

                int countratings = reviews.Count;
                int sumrating = reviews.Sum(x => x.Rating ?? 0); // Rating может быть nullable
                int average = countratings > 0 ? sumrating / countratings : 0;

                int countbad = reviews.Count(r => (r.Rating ?? 0) <= 3);
                int countgood = reviews.Count(r => (r.Rating ?? 0) > 3);
                // Обновление или вставка записи в AnalyzedData
                var existing = Db.AnalyzedData.FirstOrDefault(x => x.IdDoctor == doctor.DoctorId);

                if (existing == null)
                {
                    AnalyzedDatum analyze = new AnalyzedDatum()
                    {
                        IdDoctor = doctor.DoctorId,
                        PatientsCounted = countperson,
                        MoneyCounted = countmoney,
                        HoursInWork = hrs/60,
                        Averagerating = average,
                        Countbadrating = countbad,
                        Countgoodrating = countgood,
                    };
                    Db.AnalyzedData.Add(analyze);
                }
                else
                {
                    existing.PatientsCounted = countperson;
                    existing.MoneyCounted = countmoney;
                    existing.HoursInWork = hrs/60;
                    existing.Averagerating = average;
                    existing.Countbadrating = countbad;
                    existing.Countgoodrating = countgood;
                    Db.AnalyzedData.Update(existing);
                }
            }

            Db.SaveChanges();
        }
    }
}
