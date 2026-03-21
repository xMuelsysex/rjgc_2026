using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class PatientOrder : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _orderType = "长期医嘱";
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private string _doctorName = string.Empty;
    [ObservableProperty] private string _executionStatus = "待执行";
    [ObservableProperty] private DateTime _startDate = DateTime.Now;
    [ObservableProperty] private string _notes = string.Empty;
}
