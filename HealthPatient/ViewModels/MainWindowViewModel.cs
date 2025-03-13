﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthPatient.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] ViewModelBase pageSwitcher;

        public MainWindowViewModel Instance { get; set; }
        public MainWindowViewModel() 
        {
            Instance = this;
            PageSwitcher = new LoginViewModel();
        }
    }
}
