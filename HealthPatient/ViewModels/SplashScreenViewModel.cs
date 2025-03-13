using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.ViewModels
{
    public partial class SplashScreenViewModel: ViewModelBase
    {
        [ObservableProperty] Bitmap image;
        private readonly DispatcherTimer _splashTimer;


        public SplashScreenViewModel()
        {
            image = ConvertLogo();
            GoToLoginViewTimer();
        }
       
        public Bitmap ConvertLogo()
        {
            
           Uri uri = new Uri($"avares://HealthPatient/Assets/logo.png");
            return new Bitmap(AssetLoader.Open(uri));
        }

        public void GoToLoginViewTimer()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();

                // Проверка на null для безопасности
                if (MainWindowViewModel.Instance != null)
                {
                    // Выполняем в UI потоке
                    Dispatcher.UIThread.Post(() =>
                    {
                        MainWindowViewModel.Instance.PageSwitcher = new LoginViewModel();
                    });
                }
            };

            timer.Start();
        }
    }
}
