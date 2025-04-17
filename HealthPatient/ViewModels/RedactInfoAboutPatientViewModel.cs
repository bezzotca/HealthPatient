using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace HealthPatient.ViewModels
{
    public partial class RedactInfoAboutPatientViewModel: ViewModelBase
    {
        [ObservableProperty] Patient patient;
        [ObservableProperty] string image;
        [ObservableProperty] string message;

        public RedactInfoAboutPatientViewModel()
        {
            patient = MainWindowViewModel.Instance.Patient;
        }

        public void Save(Patient patient)
        {
            patient.UpdatedAt = DateTime.Now;   
            Db.Patients.Update(patient);
            Db.SaveChanges();
            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new PatientsMainViewModel();
        }
        [RelayCommand]
        public async Task SelectImageAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Изображения", Extensions = new List<string> { "jpg", "jpeg", "png", "bmp" } },
            new FileDialogFilter { Name = "Все файлы", Extensions = new List<string> { "*" } }
        }
            };

            // Получаем текущее окно верхнего уровня
            var topLevel = TopLevel.GetTopLevel(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : throw new InvalidOperationException("Unable to get the main window."));

            if (topLevel is Window mainWindow)
            {
                var result = await dialog.ShowAsync(mainWindow);

                if (result != null && result.Length > 0)
                {
                    string selectedFilePath = result[0];

                    // Проверяем существование файла
                    if (!File.Exists(selectedFilePath))
                    {
                        return;
                    }

                    string fileName = Path.GetFileName(selectedFilePath);
                    fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_"); // Очищаем имя файла от недопустимых символов

                    // Определяем путь к корневой директории проекта
                    string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

                    // Определяем папку назначения
                    string destinationFolder;
                    destinationFolder = Path.Combine(projectDirectory, "Assets", "Media", "Doctors_media");

                    // Полный путь для сохранения файла
                    string destinationPath = Path.Combine(destinationFolder, fileName);
                    Console.WriteLine($"Copying file from: {selectedFilePath}");
                    Console.WriteLine($"Copying file to: {destinationPath}");

                    try
                    {
                        // Если уже есть сохраненный файл, удаляем его
                        if (!string.IsNullOrEmpty(Image))
                        {
                            string previousFilePath = Path.Combine(destinationFolder, Image);
                            if (File.Exists(previousFilePath))
                            {
                                File.Delete(previousFilePath);
                                Console.WriteLine($"Previous file deleted: {previousFilePath}");
                            }
                        }

                        // Копируем новый файл
                        File.Copy(selectedFilePath, destinationPath, overwrite: true);
                        Image = fileName; // Сохраняем имя нового файла
                    }
                    catch (FileNotFoundException)
                    {
                        message = "Исходный файл не найден.";
                    }
                    catch (IOException ex)
                    {
                        message = $"Ошибка ввода-вывода: {ex.Message}";
                    }
                    catch (UnauthorizedAccessException)
                    {
                        message = "Нет прав для записи в указанную папку.";
                    }
                    catch (Exception ex)
                    {
                        message = $"Неизвестная ошибка: {ex.Message}";
                    }
                }
                else
                {
                    message = "Файл не выбран.";
                }
            }
            else
            {
                message = "Не удалось получить главное окно.";
            }
        }
        [RelayCommand]
        public void Delete(Patient patient)
        {
            try
            {
                // Определяем путь к корневой директории проекта
                string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
                string destinationFolder = Path.Combine(projectDirectory, "Assets", "Media", "Patients_media");

                // Полный путь к файлу
                if (!string.IsNullOrEmpty(patient.Image))
                {
                    string destinationPath = Path.Combine(destinationFolder, patient.Image);
                    Console.WriteLine($"Checking file path: {destinationPath}");

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                        Console.WriteLine($"Previous file deleted: {destinationPath}");
                    }
                    else
                    {
                        Console.WriteLine($"File does not exist at path: {destinationPath}");
                    }
                }

                // Удаляем пациента из базы данных
                Db.Patients.Remove(patient);
                Db.SaveChanges();

                // Обновляем интерфейс
                MainWindowViewModel.Instance.PageSwitcherAdminPanel = new PatientsMainViewModel();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет прав для удаления файла.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
            }
        }
    }
}
