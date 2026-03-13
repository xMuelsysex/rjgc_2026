using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class Doctor : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _gender = "男";
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
}
