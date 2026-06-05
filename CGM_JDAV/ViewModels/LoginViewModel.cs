using System.Threading.Tasks;
using CGM_JDAV.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CGM_JDAV.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private SupabaseService supabaseService = new();

    [ObservableProperty] private string usuario = string.Empty;
    [ObservableProperty] private string contrasena = string.Empty;
    [ObservableProperty] private string mensajeError = string.Empty;
    [ObservableProperty] private bool hayError;

    public LoginViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            MensajeError = "Por favor, rellena todos los campos.";
            HayError = true;
            return;
        }

        var lista = await supabaseService.ComprobarLogin(Usuario, Contrasena);

        if (lista != null && lista.Count > 0)
        {
            HayError = false;
            _navigationService.NavigateTo(NavigationService.MENU_VIEW);
        }
        else
        {
            MensajeError = "Usuario o contraseña incorrectos.";
            HayError = true;
        }
    }
}