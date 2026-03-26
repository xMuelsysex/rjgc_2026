using System;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "HospitalWard",
        "app-debug.log");

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _displayName = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _idCard = string.Empty;
    [ObservableProperty] private SystemRole _selectedRole = SystemRole.Patient;
    [ObservableProperty] private string _statusMessage = "请选择角色并填写注册信息。";

    public SystemRole[] AvailableRoles { get; } = [SystemRole.Patient, SystemRole.Doctor, SystemRole.Nurse];

    public bool ShowDepartment => SelectedRole is SystemRole.Doctor or SystemRole.Nurse;
    public bool ShowIdCard => SelectedRole == SystemRole.Patient;

    partial void OnSelectedRoleChanged(SystemRole value)
    {
        OnPropertyChanged(nameof(ShowDepartment));
        OnPropertyChanged(nameof(ShowIdCard));
    }

    [RelayCommand]
    private void Register()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)!);
        File.AppendAllText(
            LogFilePath,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register start. Username={Username}, Role={SelectedRole}, DisplayName={DisplayName}, Department={Department}, IdCardLength={IdCard?.Length ?? 0}{Environment.NewLine}");

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(DisplayName))
        {
            StatusMessage = "账号、密码、姓名不能为空。";
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register validation failed: required fields missing.{Environment.NewLine}");
            return;
        }

        if (Password != ConfirmPassword)
        {
            StatusMessage = "两次密码输入不一致。";
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register validation failed: password mismatch.{Environment.NewLine}");
            return;
        }

        if (SelectedRole == SystemRole.Patient && string.IsNullOrWhiteSpace(IdCard))
        {
            StatusMessage = "患者注册必须填写身份证号。";
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register validation failed: patient id card missing.{Environment.NewLine}");
            return;
        }

        if ((SelectedRole == SystemRole.Doctor || SelectedRole == SystemRole.Nurse) && string.IsNullOrWhiteSpace(Department))
        {
            StatusMessage = "医生和护士注册必须填写科室。";
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register validation failed: department missing for staff role.{Environment.NewLine}");
            return;
        }

        if (_ds.UserCredentials.Any(x => x.Username == Username))
        {
            StatusMessage = "账号已存在。";
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register validation failed: duplicate username.{Environment.NewLine}");
            return;
        }

        try
        {
            var roleText = SelectedRole.ToString();
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register before add credential/profile.{Environment.NewLine}");

            _ds.UserCredentialRepo.Add(new UserCredential
            {
                Id = _ds.GenerateId(),
                Role = roleText,
                Username = Username,
                Password = Password,
                DisplayName = DisplayName,
                Department = Department,
                Phone = Phone,
                IdCard = IdCard
            });

            _ds.UserProfileRepo.Add(new UserProfile
            {
                Id = _ds.GenerateId(),
                Role = roleText,
                DisplayName = DisplayName,
                Department = Department,
                Phone = Phone,
                Email = $"{Username}@hospital.local",
                Bio = $"{SelectedRole.GetDisplayName()}注册用户"
            });

            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register before SaveChanges.{Environment.NewLine}");
            _ds.SaveChanges();
            File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register SaveChanges succeeded.{Environment.NewLine}");

            StatusMessage = "注册成功，请返回登录。";
        }
        catch (Exception ex)
        {
            File.AppendAllText(
                LogFilePath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Register exception: {ex}{Environment.NewLine}");
            throw;
        }
    }
}
