using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthPatient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        [ObservableProperty] string message;
        [ObservableProperty] List<Gender> genders;
        [ObservableProperty] Gender selectedGender;
        [ObservableProperty] string image;
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
                        Message = "Выбранный файл не существует.";
                        return;
                    }

                    string fileName = Path.GetFileName(selectedFilePath);
                    fileName = Regex.Replace(fileName, @"[<>:""/\\|?*]", "_"); // Очищаем имя файла от недопустимых символов

                    // Определяем путь к корневой директории проекта
                    string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

                    // Определяем папку назначения
                    string destinationFolder;
                    if (ChangedFilter == "Врач")
                    {
                        destinationFolder = Path.Combine(projectDirectory, "Assets", "Media", "Doctors_media");
                    }
                    else
                    {
                        destinationFolder = Path.Combine(projectDirectory, "Assets", "Media", "Patients_media");
                    }

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
                        Message = $"Файл успешно скопирован: {fileName}";
                    }
                    catch (FileNotFoundException)
                    {
                        Message = "Исходный файл не найден.";
                    }
                    catch (IOException ex)
                    {
                        Message = $"Ошибка ввода-вывода: {ex.Message}";
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Message = "Нет прав для записи в указанную папку.";
                    }
                    catch (Exception ex)
                    {
                        Message = $"Неизвестная ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Message = "Файл не выбран.";
                }
            }
            else
            {
                Message = "Не удалось получить главное окно.";
            }
        }
        bool IsPasswordValid()
        {

            if (Password.Length < 6)
                return false;

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in Password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                if (char.IsLower(c)) hasLowerCase = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (Regex.IsMatch(c.ToString(), @"[\W_]")) hasSpecialChar = true;
            }

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        bool IsEmailValid()
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(Email, emailPattern);
        }

        public void CreateUser()
        {
            Message = null;
            switch (ChangedFilter)
            {
                case "Врач":
                    if (FirstName != null && FirstName != "" && LastName != null
                        && LastName != "" && Patronymic != null && Patronymic != "" && Bio != null && Bio != ""
                        && Login != null && Login != "" && Password != null && Password != "" && SelectedGender != null)
                    {
                        if (!IsPasswordValid())
                        {
                            Message = "Введите надёжный пароль";
                        }
                        else
                        {
                            Doctor doctor = new Doctor()
                            {
                                DoctorId = Db.Doctors.Select(x => x.DoctorId).Max() + 1,
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
                            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new AdminRightsViewModel();
                        }
                    }
                    else
                    {
                        Message = "Не все поля заполнены данными";
                    }
                    
                    break;

                case "Пациент":
                    if (FirstName != null && FirstName != "" && LastName != null
                        && LastName != "" && Patronymic != null && Patronymic != ""
                        && Login != null && Login != "" && Password != null && Password != ""
                        && Email != null && Email != "" && ContactPhone != null && ContactPhone != "" && SelectedGender != null)
                    {
                        if (!IsPasswordValid())
                        {
                            Message = "Введите надёжный пароль";
                        }
                        else if(!IsEmailValid())
                        {
                            Message = "Введите корректно почту";
                        }
                        else
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
                                Image = Image,
                                GenderId = SelectedGender.GenderId,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                            };
                            Db.Patients.Add(patient);
                            Db.SaveChanges();
                            MainWindowViewModel.Instance.PageSwitcherAdminPanel = new AdminRightsViewModel();
                        }
                    }
                    else
                    {
                        Message = "Не все поля заполнены";
                    }
                    break;
            }
        }
    }
}
