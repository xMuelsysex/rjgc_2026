using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly UserSessionService _sessionService = UserSessionService.Instance;

    [ObservableProperty] private string _username = "admin";
    [ObservableProperty] private string _password = "admin";
    [ObservableProperty] private string _statusMessage = "请输入账号密码登录。";
    [ObservableProperty] private bool _loginSucceeded;

    [RelayCommand]
    private void Login()
    {
        LoginSucceeded = _sessionService.Login(Username, Password, out var message);
        StatusMessage = message;
    }
}
