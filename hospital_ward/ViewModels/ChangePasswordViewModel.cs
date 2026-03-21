using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class ChangePasswordViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private UserCredential? _credential;

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _oldPassword = string.Empty;
    [ObservableProperty] private string _newPassword = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _statusMessage = "请输入账号和密码信息。";

    public ChangePasswordViewModel()
    {
        _credential = _ds.UserCredentials.FirstOrDefault(c => c.Role == "Patient") ?? _ds.UserCredentials.FirstOrDefault();
        Username = _credential?.Username ?? string.Empty;
    }

    [RelayCommand]
    private void Save()
    {
        if (_credential is null)
        {
            StatusMessage = "未找到账号信息。";
            return;
        }

        if (_credential.Password != OldPassword)
        {
            StatusMessage = "原密码不正确。";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmPassword)
        {
            StatusMessage = "新密码为空或两次输入不一致。";
            return;
        }

        _credential.Password = NewPassword;
        _ds.SaveChanges();
        StatusMessage = "密码修改成功。";
        OldPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
    }
}
