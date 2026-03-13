using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFirstApp.Models;

public partial class Patient : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _gender = "男";
    [ObservableProperty] private int _age;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _idCard = string.Empty;
    [ObservableProperty] private DateTime _admissionDate = DateTime.Now;
    [ObservableProperty] private string _bedNumber = string.Empty;
    [ObservableProperty] private string _departmentName = string.Empty;
    [ObservableProperty] private string _status = "在院";
}
