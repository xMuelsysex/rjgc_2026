using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFirstApp.Models;

public partial class DischargeRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _bedNumber = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private DateTime _dischargeDate = DateTime.Now;
    [ObservableProperty] private decimal _totalCost;
}
