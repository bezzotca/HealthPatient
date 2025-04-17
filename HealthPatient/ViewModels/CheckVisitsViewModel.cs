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
using Avalonia.Media.TextFormatting;
using System.Diagnostics;
using Tmds.DBus.Protocol;

namespace HealthPatient.ViewModels
{
    public partial class CheckVisitsViewModel: ViewModelBase
    {
        [ObservableProperty] Doctor doctor = MainWindowViewModel.Instance.Doctor;
        [ObservableProperty] Patient patient = MainWindowViewModel.Instance.Patient;
        [ObservableProperty] List<Visit> visits;
        [ObservableProperty] List<Visit> visits0;
        [ObservableProperty] string sum;
        [ObservableProperty] string count;
        [ObservableProperty] string msg;
        [ObservableProperty] bool isVisible;
        [ObservableProperty] bool isVisibleButton;
        [ObservableProperty] List<string> filter;
        [ObservableProperty] string changedFilter;
        [ObservableProperty] string textFind;
        public CheckVisitsViewModel()
        {
            isVisible = false;
            isVisibleButton = false;
            if (doctor != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.DoctorId == doctor.DoctorId).ToList();
                visits0 = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.DoctorId == doctor.DoctorId).ToList();
            }
            else if(patient != null)
            {
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).Where(x => x.PatientId == patient.PatientId).ToList();
            }
            else if(doctor == null && patient == null)
            {
                isVisible = true;
                visits = Db.Visits.Include(x => x.Patient).Include(x => x.Schedule).Include(x => x.Doctor).ThenInclude(x=>x.ServicePrices).ToList();
                sum = Db.Visits.Select(x => x.Cost).Sum().ToString();
                count = Db.Visits.ToList().Count().ToString();
            }
            if(MainWindowViewModel.Instance.PrevPage == "DoctorWorkViewModel" || MainWindowViewModel.Instance.PrevPage == "PatientWorkViewModel")
            {
                isVisibleButton = true;
            }
            filter = new List<string>
            {
                "Без фильтра",
                "Диагноз",
                "Статус",
                "Рекоммендации",
            };
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
            if(MainWindowViewModel.Instance.PrevPage == "DoctorWorkViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new DoctorWorkViewModel();
            }
            else if(MainWindowViewModel.Instance.PrevPage == "PatientWorkViewModel")
            {
                MainWindowViewModel.Instance.PageSwitcher = new PatientWorkViewModel();
            }
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
                Msg = $"Ошибка при генерации PDF: {ex.Message}";
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
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Дата");
                                header.Cell().Text("Пациент");
                                header.Cell().Text("Врач");
                                header.Cell().Text("Диагноз");
                                header.Cell().Text("Стоимость");

                                header.Cell().ColumnSpan(4)
                                    .PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            foreach (var visit in visits)
                            {
                                table.Cell().Text(visit.VisitDate?.ToString("dd.MM.yyyy"));
                                table.Cell().Text(visit.Patient?.FirstName);
                                table.Cell().Text(visit.Doctor?.FirstName);
                                table.Cell().Text(visit.Diagnosis);
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
            // Определяем базовую директорию проекта
            string baseDirectory = AppContext.BaseDirectory;
            string reportsDirectory = Path.Combine(baseDirectory, @"..\..\..\Assets\Reports");

            // Создаем подпапку для пациента или врача
            string userFolder = Doctor != null ? $"Врач_{Doctor.LastName}" :
                                Patient != null ? $"Пациент_{Patient.LastName}" :
                                null;

            if (!string.IsNullOrEmpty(userFolder))
            {
                reportsDirectory = Path.Combine(reportsDirectory, userFolder);
            }

            // Создаем папку, если она не существует
            Directory.CreateDirectory(reportsDirectory);

            // Генерируем имя файла
            var prefix = Doctor != null ? $"Врач_{Doctor.LastName}" :
                       Patient != null ? $"Пациент_{Patient.LastName}" :
                       "Все_визиты";

            string fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            // Возвращаем полный путь к файлу
            return Path.Combine(reportsDirectory, fileName);
        }

        [RelayCommand]
        public void OpenFolder()
        {
            try
            {
                // Получаем путь к папке
                string folderPath = GetFolderPath();

                // Проверяем, существует ли папка
                if (Directory.Exists(folderPath))
                {
                    // Открываем папку через проводник Windows
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = folderPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                else
                {
                    Msg = "Папка не существует.";
                }
            }
            catch (Exception ex)
            {
                Msg = $"Ошибка при открытии папки: {ex.Message}";
            }
        }

        private string GetFolderPath()
        {
            // Определяем базовую директорию проекта
            string baseDirectory = AppContext.BaseDirectory;
            string reportsDirectory = Path.Combine(baseDirectory, @"..\..\..\Assets\Reports");

            // Если пользователь - пациент или врач, открываем его папку
            if (Doctor != null)
            {
                return Path.Combine(reportsDirectory, $"Врач_{Doctor.LastName}");
            }
            else if (Patient != null)
            {
                return Path.Combine(reportsDirectory, $"Пациент_{Patient.LastName}");
            }
            else
            {
                // Если пользователь - администратор, открываем общую папку Reports
                return reportsDirectory;
            }
        }
        partial void OnTextFindChanged(string value)
        {
            if (ChangedFilter == "Без фильтра")
            {
                Visits = Db.Visits.ToList();
            }
            else if (ChangedFilter != "Без фильтра")
            {
                if (value == "" || value == null)
                {

                }
                else if (value != "")
                {
                    switch (ChangedFilter)
                    {
                        case "Диагноз":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x=>x.Diagnosis.Contains(value)).ToList();
                            break;
                        case "Статус":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x => x.Status.Contains(value)).ToList();
                            break;
                        case "Рекоммендации":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x => x.Prescriptions.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }

        
        partial void OnChangedFilterChanged(string value)
        {
            if (value == "Без фильтра")
            {

            }
            else if (value != "Без фильтра")
            {
                if (TextFind == "" || TextFind == null)
                {

                }
                else if (TextFind != "")
                {
                    switch (value)
                    {
                        case "Диагноз":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x => x.Diagnosis.Contains(value)).ToList();
                            break;
                        case "Статус":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x => x.Status.Contains(value)).ToList();
                            break;
                        case "Рекоммендации":
                            Visits = visits0;
                            Visits = Db.Visits.Where(x => x.Prescriptions.Contains(value)).ToList();
                            break;
                    }
                }
            }
        }
    }
}
