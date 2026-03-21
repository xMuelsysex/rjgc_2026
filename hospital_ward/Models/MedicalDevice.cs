using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class MedicalDevice : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _deviceName = string.Empty;
    [ObservableProperty] private string _modelNumber = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _manager = string.Empty;
    [ObservableProperty] private DateTime _purchaseDate = DateTime.Now;
    [ObservableProperty] private string _status = "在用";
    [ObservableProperty] private string _maintenanceCycle = string.Empty;
}
