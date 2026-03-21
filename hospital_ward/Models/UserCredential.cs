using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class UserCredential : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _role = string.Empty;
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _displayName = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _idCard = string.Empty;
}
