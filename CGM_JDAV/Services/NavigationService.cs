using Avalonia.Controls;
using CGM_JDAV.ViewModels;
using CGM_JDAV.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CGM_JDAV.Services;

public partial class NavigationService : ObservableObject
{
    public const string LOGIN_VIEW = "login";
    public const string MENU_VIEW = "menu";

    [ObservableProperty]
    private ContentControl currentView;

    public NavigationService()
    {
        NavigateTo(LOGIN_VIEW);
    }

    public void NavigateTo(string tag)
    {
        if (tag.Equals(LOGIN_VIEW))
        {
            LoginView loginView = new LoginView();
            loginView.DataContext = new LoginViewModel(this);
            CurrentView = loginView;
        }
        else if (tag.Equals(MENU_VIEW))
        {
            MenuPrincipal menuView = new MenuPrincipal();
            menuView.DataContext = new MenuPrincipalViewModel(this);
            CurrentView = menuView;
        }
        
    }
}