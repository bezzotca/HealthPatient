using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HealthPatient.ViewModels
{
    public partial class CheckVisitsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor = MainWindowViewModel.Instance.Doctor;
        [ObservableProperty] Patient patient = MainWindowViewModel.Instance.Patient;
        [ObservableProperty] List<Visit> visits;
        [ObservableProperty] string sum;
        [ObservableProperty] string count;
        [ObservableProperty] string msg;
        [ObservableProperty] bool isVisible;
        [ObservableProperty] bool isVisibleButton;
        public CheckVisitsViewModel()
        {
            isVisible = false;
            isVisibleButton = false;
            if (doctor != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.DoctorId == doctor.DoctorId).ToList();
            }
            else if(patient != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.PatientId == patient.PatientId).ToList();
            }
            else if(doctor == null && patient == null)
            {
                isVisible = true;
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).ToList();
                sum = Db.Visits.Select(x => x.Cost).Sum().ToString();
                count = Db.Visits.ToList().Count().ToString();
            }
            if(MainWindowViewModel.Instance.PrevPage == "DoctorWorkViewModel")
            {
                isVisibleButton = true;
            }
        }

        public void Save(Visit visit)
        {
            if(visit.Patient != null)
            {
                visit.Patient.UpdatedAt = DateTime.Now;
            }

            if (visit.Doctor != null)
            {
                visit.Doctor.UpdatedAt = DateTime.Now;
            }
            visit.UpdatedAt = DateTime.Now;
            Db.Visits.Update(visit);
            Db.SaveChanges();
        }

        public void GoBack()
        {
            MainWindowViewModel.Instance.PageSwitcher = new DoctorWorkViewModel();
        }

        [RelayCommand]
        public void GeneratePdf()
        {
            try
            {
                var pdfBytes = GenerateVisitsPdf(Visits);
                var fileName = GetPdfFileName();
                File.WriteAllBytes(fileName, pdfBytes);
                Msg = "Отчет о посещениях сохранён";
            }
            catch (Exception ex)
            {
                // Обработка ошибок
            }
        }

        public byte[] GenerateVisitsPdf(List<Visit> visits)
        {
            if (visits == null || !visits.Any())
                throw new InvalidOperationException("Нет данных для формирования отчета");

            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("Отчет о приёмах")
                        .Bold().FontSize(20);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Дата");
                                header.Cell().Text("Пациент");
                                header.Cell().Text("Врач");
                                header.Cell().Text("Стоимость");

                                header.Cell().ColumnSpan(4)
                                    .PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            foreach (var visit in visits)
                            {
                                table.Cell().Text(visit.VisitDate?.ToString("dd.MM.yyyy"));
                                table.Cell().Text(visit.Patient?.FirstName);
                                table.Cell().Text(visit.Doctor?.FirstName);
                                table.Cell().Text($"{visit.Cost?.ToString("N2")} ₽");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Страница ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
        }

        public string GetPdfFileName()
        {
            var prefix = Doctor != null ? $"Врач_{Doctor.LastName}" :
                       Patient != null ? $"Пациент_{Patient.LastName}" :
                       "Все_визиты";

            return $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        }
    }
}
