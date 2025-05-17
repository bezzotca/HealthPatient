using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;
using Microsoft.EntityFrameworkCore;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using ClosedXML.Excel;

namespace HealthPatient.ViewModels
{
    public partial class AdminRightsViewModel: ViewModelBase
    {
        [ObservableProperty] string helloMessage;
        [ObservableProperty] string mrMrs;
        [ObservableProperty] Administrator admin;
        public AdminRightsViewModel()
        {
            admin = MainWindowViewModel.Instance.Admin;
            GenerateGreeting();
            MrsOrMs();
        }

        public void GenerateGreeting()
        {
            int hour = DateTime.Now.Hour;
            string timeOfDay = hour switch
            {
                >= 9 and <= 11 => "Утро",
                >= 11 and < 18 => "День",
                _ => "Вечер"
            };

            HelloMessage = $"Добрый {timeOfDay}!";
        }

        public void MrsOrMs()
        {
            if (admin.GenderId== 1)
            {
                MrMrs = "Мистер";
            }
            else
            {
                MrMrs = "Миссис";
            }
        }

        public void Redact()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new RedactInfoAboutAdminViewModel();
        }
        public void AddUser()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new CreateUserViewModel();
        }
        public void AddNews()
        {
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new AddNewViewModel();
        }

        [RelayCommand]
        public async Task ImportPatientsFromExcelAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Импорт пациентов",
                Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Excel файлы", Extensions = new List<string> { "xlsx", "xls" } },
            new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
        }
            };

            // Получаем главное окно приложения
            var topLevel = TopLevel.GetTopLevel(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : throw new InvalidOperationException("Не удалось получить главное окно приложения."));

            if (topLevel is not Window mainWindow)
            {
                // Можно показать сообщение или лог
                return;
            }

            var result = await dialog.ShowAsync(mainWindow);
            if (result == null || result.Length == 0)
            {
                // Пользователь не выбрал файл
                return;
            }

            string selectedFilePath = result[0];
            if (!File.Exists(selectedFilePath))
                return;

            // Путь в папке проекта для сохранения копии файла
            string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string destinationFolder = Path.Combine(projectDir, "Assets", "Import", "Import_Patients");

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string fileName = Path.GetFileName(selectedFilePath);
            fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_"); // Очищаем имя файла от недопустимых символов

            string destinationFilePath = Path.Combine(destinationFolder, fileName);

            try
            {
                File.Copy(selectedFilePath, destinationFilePath, overwrite: true);
            }
            catch (Exception ex)
            {
                // Обработать ошибки копирования
                return;
            }

            try
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook(destinationFilePath);
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропускаем заголовок

                foreach (var row in rows)
                {
                    // Читаем Id (число из второй колонки)
                    if (!int.TryParse(row.Cell(2).GetString(), out int id))
                        continue;

                    // Проверяем, нет ли уже пациента с таким Id в базе
                    if (Db.Patients.Any(p => p.PatientId == id))
                        continue;

                    // Парсим другие данные
                    DateTime.TryParse(row.Cell(3).GetString(), out DateTime dateAndTime);
                    string clientPhone = row.Cell(4).GetString();
                    string clientEmail = row.Cell(5).GetString();
                    string formName = row.Cell(6).GetString();

                    string clientFullName = row.Cell(7).GetString();
                    string clientSurname = row.Cell(8).GetString();
                    string clientName = row.Cell(9).GetString();
                    string clientPatronymic = row.Cell(10).GetString();

                    DateTime.TryParse(row.Cell(11).GetString(), out DateTime planStart);
                    DateTime.TryParse(row.Cell(12).GetString(), out DateTime planEnd);

                    string comment = row.Cell(13).GetString();

                    int.TryParse(row.Cell(14).GetString(), out int doctorId);
                    string doctorName = row.Cell(15).GetString();

                    if(Db.Doctors.Where(x=>x.DoctorId == doctorId).FirstOrDefault() == null)
                    {
                        continue;
                    }
                    // Проверка времени: PlanEnd >= PlanStart, длительность <= 12 часов
                    if (planEnd < planStart || (planEnd - planStart).TotalHours > 12)
                        continue;

                    // Логика заполнения Patient
                    var patient = new Patient
                    {
                        PatientId = id,
                        ContactPhone = clientPhone,
                        FirstName = clientName,
                        LastName = clientSurname,
                        Patronymic = clientPatronymic,
                        Email = clientEmail,
                        Login = clientName,
                        Password = clientPatronymic
                    };

                    
                    Db.Patients.Add(patient);

                    if(Db.Schedules.Where(x => x.DatestartSchedule == planStart).Select(x => x.ScheduleId).FirstOrDefault() == null)
                    {
                        continue;
                    }
                    else
                    {
                        var visit = new Visit
                        {
                            PatientId = id,
                            DoctorId = doctorId,
                            VisitDate = planStart,
                            Status = "зарегистрировано",
                            ScheduleId = Db.Schedules.Where(x => x.DatestartSchedule == planStart).Select(x => x.ScheduleId).FirstOrDefault(),
                        };
                        Db.Visits.Add(visit);
                    }
                    
                   
                }

                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Обработать ошибки парсинга и добавления
            }
        }

        [RelayCommand]
        public async Task ImportDoctorsFromExcelAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Импорт врачей",
                Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Excel файлы", Extensions = new List<string> { "xlsx", "xls" } },
            new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
        }
            };

            var topLevel = TopLevel.GetTopLevel(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : throw new InvalidOperationException("Не удалось получить главное окно приложения."));

            if (topLevel is not Window mainWindow)
            {
                return;
            }

            var result = await dialog.ShowAsync(mainWindow);
            if (result == null || result.Length == 0)
            {
                return;
            }

            string selectedFilePath = result[0];
            if (!File.Exists(selectedFilePath))
                return;

            string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string destinationFolder = Path.Combine(projectDir, "Assets", "Import", "Import_Doctors");

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string fileName = Path.GetFileName(selectedFilePath);
            fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");

            string destinationFilePath = Path.Combine(destinationFolder, fileName);

            try
            {
                File.Copy(selectedFilePath, destinationFilePath, overwrite: true);
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook(destinationFilePath);
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропускаем заголовок

                foreach (var row in rows)
                {
                    if (!int.TryParse(row.Cell(1).GetString(), out int doctorId))
                        continue;

                    if (Db.Doctors.Any(d => d.DoctorId == doctorId))
                        continue;

                    string fullName = row.Cell(2).GetString()?.Trim();
                    if (string.IsNullOrWhiteSpace(fullName))
                        continue;

                    // Разбиваем ФИО по пробелу
                    string lastName = null;
                    string firstName = null;
                    string patronymic = null;

                    var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (nameParts.Length == 1)
                    {
                        lastName = nameParts[0];
                    }
                    else if (nameParts.Length == 2)
                    {
                        lastName = nameParts[0];
                        firstName = nameParts[1];
                    }
                    else if (nameParts.Length >= 3)
                    {
                        lastName = nameParts[0];
                        firstName = nameParts[1];
                        patronymic = nameParts[2];
                    }

                    var doctor = new Doctor
                    {
                        DoctorId = doctorId,
                        LastName = lastName,
                        FirstName = firstName,
                        Patronymic = patronymic,
                        Login = firstName,
                        Password = patronymic
                    };

                    Db.Doctors.Add(doctor);
                }

                await Db.SaveChangesAsync();
            }
            catch (Exception)
            {
                
            }
        }

        [RelayCommand]
        public async Task ExportDoctorsToExcelAsync()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Экспорт врачей",
                Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Excel файлы", Extensions = new List<string> { "xlsx" } }
        },
                DefaultExtension = "xlsx"
            };

            var topLevel = TopLevel.GetTopLevel(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : throw new InvalidOperationException("Не удалось получить главное окно приложения."));

            if (topLevel is not Window mainWindow)
            {
                return;
            }

            var result = await dialog.ShowAsync(mainWindow);
            if (string.IsNullOrEmpty(result))
            {
                // Пользователь отменил сохранение
                return;
            }

            try
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Doctors");

                // Заголовки
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "ФИО";

                var doctors = Db.Doctors.ToList();

                int currentRow = 2;
                foreach (var doc in doctors)
                {
                    worksheet.Cell(currentRow, 1).Value = doc.DoctorId;

                    // Формируем ФИО через пробел, учитывая, что Patronymic может быть null
                    string fullName = $"{doc.LastName ?? ""} {doc.FirstName ?? ""} {doc.Patronymic ?? ""}".Trim();

                    worksheet.Cell(currentRow, 2).Value = fullName;

                    currentRow++;
                }

                // Автоширина для столбцов
                worksheet.Columns().AdjustToContents();

                // Путь к папке в проекте
                string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
                string exportFolder = Path.Combine(projectDir, "Assets", "Export", "Export_Doctors");

                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);

                // Формируем имя файла с датой-временем
                string fileName = $"Doctors_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                string fullPath = Path.Combine(exportFolder, fileName);

                workbook.SaveAs(fullPath);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
            }
        }

        [RelayCommand]
        public async Task ImportDoctorSchedulesFromExcelAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Импорт расписания врачей",
                Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Excel файлы", Extensions = new List<string> { "xlsx", "xls" } },
            new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
        }
            };

            // Получаем главное окно приложения
            var topLevel = TopLevel.GetTopLevel(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : throw new InvalidOperationException("Не удалось получить главное окно приложения."));

            if (topLevel is not Window mainWindow)
            {
                return;
            }

            var result = await dialog.ShowAsync(mainWindow);
            if (result == null || result.Length == 0)
            {
                return;
            }

            string selectedFilePath = result[0];
            if (!File.Exists(selectedFilePath))
                return;

            // Путь в папке проекта для сохранения копии файла
            string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string destinationFolder = Path.Combine(projectDir, "Assets", "Import", "Import_Schedule");

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string fileName = Path.GetFileName(selectedFilePath);
            fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_"); // Очищаем имя файла от недопустимых символов

            string destinationFilePath = Path.Combine(destinationFolder, fileName);

            try
            {
                File.Copy(selectedFilePath, destinationFilePath, overwrite: true);
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook(destinationFilePath);
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропускаем заголовок

                foreach (var row in rows)
                {
                    if (!int.TryParse(row.Cell(1).GetString(), out int branchId))
                        continue;

                    if (!int.TryParse(row.Cell(2).GetString(), out int doctorId))
                        continue;

                    if (!DateTime.TryParse(row.Cell(3).GetString(), out DateTime startDateTime))
                        continue;

                    if (!int.TryParse(row.Cell(4).GetString(), out int lengthInMinutes))
                        continue;

                    string isBusyStr = row.Cell(5).GetString();
                    bool isBusy = isBusyStr == "1" || isBusyStr.ToLower() == "true";

                    // Проверяем, что такой врач и филиал есть в базе
                    if (!Db.Doctors.Any(d => d.DoctorId == doctorId))
                        continue;

                    // Проверка, есть ли уже такой слот в базе, например по BranchId, DoctorId, StartDateTime
                    bool slotExists = Db.Schedules.Any(s =>
                        s.DoctorId == doctorId &&
                        s.DatestartSchedule == startDateTime);

                    if (slotExists)
                        continue;

                    var scheduleSlot = new Schedule
                    {
                        DoctorId = doctorId,
                        DatestartSchedule = startDateTime,
                        Lengthinmins = lengthInMinutes,
                        IsBusy = isBusy
                    };

                    Db.Schedules.Add(scheduleSlot);
                }

                await Db.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Обработка ошибок
            }
        }

        [RelayCommand]
        public async Task ExportPatientsToExcelAsync()
        {
            try
            {
                // Получаем данные пациентов из базы
                var patients = Db.Patients.ToList();

                if (patients.Count == 0)
                    return;

                // Путь в папке проекта для сохранения файла
                string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
                string exportFolder = Path.Combine(projectDir, "Assets", "Export", "Export_Patients");

                if (!Directory.Exists(exportFolder))
                    Directory.CreateDirectory(exportFolder);

                // Формируем имя файла с датой и временем
                string fileName = $"Patients_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string filePath = Path.Combine(exportFolder, fileName);

                using var workbook = new ClosedXML.Excel.XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Patients");

                // Записываем заголовки (соответствуют структуре импорта)
                worksheet.Cell(1, 1).Value = "№ п/п";              
                worksheet.Cell(1, 2).Value = "Id";                 
                worksheet.Cell(1, 3).Value = "DateAndTime";         
                worksheet.Cell(1, 4).Value = "ClientPhone";         
                worksheet.Cell(1, 5).Value = "ClientEmail";         
                worksheet.Cell(1, 6).Value = "FormName";             
                worksheet.Cell(1, 7).Value = "ClientFullName";     
                worksheet.Cell(1, 8).Value = "ClientSurname";      
                worksheet.Cell(1, 9).Value = "ClientName";          
                worksheet.Cell(1, 10).Value = "ClientPatronymic";   
                worksheet.Cell(1, 11).Value = "PlanStart";         
                worksheet.Cell(1, 12).Value = "PlanEnd";           
                worksheet.Cell(1, 13).Value = "Comment";           
                worksheet.Cell(1, 14).Value = "DoctorId";           
                worksheet.Cell(1, 15).Value = "DoctorName";         

                int rowNumber = 2;
                int index = 1;

                foreach (var p in patients)
                {
                    worksheet.Cell(rowNumber, 1).Value = index++;
                    worksheet.Cell(rowNumber, 2).Value = p.PatientId;
                    worksheet.Cell(rowNumber, 3).Value = ""; 
                    worksheet.Cell(rowNumber, 4).Value = p.ContactPhone ?? "";
                    worksheet.Cell(rowNumber, 5).Value = p.Email ?? "";
                    worksheet.Cell(rowNumber, 6).Value = ""; 

                    // ClientFullName — объединение ФИО
                    worksheet.Cell(rowNumber, 7).Value = $"{p.LastName} {p.FirstName} {p.Patronymic}".Trim();

                    worksheet.Cell(rowNumber, 8).Value = p.LastName ?? "";
                    worksheet.Cell(rowNumber, 9).Value = p.FirstName ?? "";
                    worksheet.Cell(rowNumber, 10).Value = p.Patronymic ?? "";

                    worksheet.Cell(rowNumber, 11).Value = ""; // PlanStart
                    worksheet.Cell(rowNumber, 12).Value = ""; // PlanEnd

                    worksheet.Cell(rowNumber, 13).Value = ""; // Comment
                    worksheet.Cell(rowNumber, 14).Value = 0;  
                    worksheet.Cell(rowNumber, 15).Value = ""; // DoctorName

                    rowNumber++;
                }

                workbook.SaveAs(filePath);

                // Опционально можно вывести сообщение об успешном сохранении
            }
            catch (Exception ex)
            {
                // Обработка ошибок
            }
        }

        public void GoToDiagrams()
        {
            MainWindowViewModel.Instance.PageSwitcher = new DiagramsViewModel();
        }
    }
}
