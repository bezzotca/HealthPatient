using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;

namespace HealthPatient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] ViewModelBase pageSwitcher;
        [ObservableProperty] public Administrator admin;
        [ObservableProperty] public Patient patient;
        [ObservableProperty] public Doctor doctor;

        public static MainWindowViewModel Instance { get; set; }
        public MainWindowViewModel() 
        {
            Instance = this;
            PageSwitcher = new LoginViewModel();
        }

    }
}
