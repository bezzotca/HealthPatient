using CommunityToolkit.Mvvm.ComponentModel;
using HealthPatient.Models;

namespace HealthPatient.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty] HealthpatientContext db = new HealthpatientContext();
    }
}
