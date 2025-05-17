using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Drawing;

namespace HealthPatient.ViewModels
{
    public partial class DiagramsViewModel: ViewModelBase
    {
        [ObservableProperty] List<AnalyzedDatum> data;

        [ObservableProperty]
        public ObservableCollection<(string DoctorName, decimal? Value)> topMoneyCounted;
        [ObservableProperty]
        public ObservableCollection<(string DoctorName, int? Value)> topPatientsCounted;
        [ObservableProperty]
        public ObservableCollection<(string DoctorName, int? Value)> topHoursInWork;
        [ObservableProperty]
        public ObservableCollection<(string DoctorName, decimal? Value)> topAverageRating;
        [ObservableProperty]
        public ObservableCollection<(string DoctorName, int? Value)> topCountBadRating;
        [ObservableProperty]
        public ObservableCollection<(string DoctorName, int? Value)> topCountGoodRating;

        public DiagramsViewModel()
        {
            HealthpatientContext db = new HealthpatientContext();
            data = db.AnalyzedData.Include(x=>x.IdDoctorNavigation).ToList();
            TopMoneyCounted = new ObservableCollection<(string, decimal?)>(
            data
            .Where(x => x.MoneyCounted.HasValue)
            .OrderByDescending(x => x.MoneyCounted)
            .Take(3)
            .Select(x => (x.IdDoctorNavigation.FirstName, x.MoneyCounted)));

            TopPatientsCounted = new ObservableCollection<(string, int?)>(
                data
                .Where(x => x.PatientsCounted.HasValue)
                .OrderByDescending(x => x.PatientsCounted)
                .Take(3)
                .Select(x => (x.IdDoctorNavigation.FirstName, x.PatientsCounted)));

            TopHoursInWork = new ObservableCollection<(string, int?)>(
                data
                .Where(x => x.HoursInWork.HasValue)
                .OrderByDescending(x => x.HoursInWork)
                .Take(3)
                .Select(x => (x.IdDoctorNavigation.FirstName, x.HoursInWork)));

            TopAverageRating = new ObservableCollection<(string, decimal?)>(
                data
                .Where(x => x.Averagerating.HasValue)
                .OrderByDescending(x => x.Averagerating)
                .Take(3)
                .Select(x => (x.IdDoctorNavigation.FirstName, x.Averagerating)));

            TopCountBadRating = new ObservableCollection<(string, int?)>(
                data
                .Where(x => x.Countbadrating.HasValue)
                .OrderByDescending(x => x.Countbadrating)
                .Take(3)
                .Select(x => (x.IdDoctorNavigation.FirstName, x.Countbadrating)));

            TopCountGoodRating = new ObservableCollection<(string, int?)>(
                data
                .Where(x => x.Countgoodrating.HasValue)
                .OrderByDescending(x => x.Countgoodrating)
                .Take(3)
                .Select(x => (x.IdDoctorNavigation.FirstName, x.Countgoodrating)));
        }
        
        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new AdminMainViewModel();
        }
    }
}
