using CGM_JDAV.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CGM_JDAV.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private NavigationService navigationService = new();
    
    public string Greeting { get; } = "Welcome to Avalonia!";
}