using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class UserProfileViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _displayName = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _bio = string.Empty;
    [ObservableProperty] private string _role = string.Empty;

    private UserProfile? _profile;

    public UserProfileViewModel()
    {
        _profile = _ds.UserProfiles.FirstOrDefault();
        if (_profile is null) return;

        DisplayName = _profile.DisplayName;
        Department = _profile.Department;
        Phone = _profile.Phone;
        Email = _profile.Email;
        Bio = _profile.Bio;
        Role = _profile.Role;
    }

    [RelayCommand]
    private void Save()
    {
        if (_profile is null) return;

        _profile.DisplayName = DisplayName;
        _profile.Department = Department;
        _profile.Phone = Phone;
        _profile.Email = Email;
        _profile.Bio = Bio;
        _ds.SaveChanges();
    }
}
