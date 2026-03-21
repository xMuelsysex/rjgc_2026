using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class NursingRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _nurseName = string.Empty;
    [ObservableProperty] private string _careLevel = "一级护理";
    [ObservableProperty] private string _taskName = string.Empty;
    [ObservableProperty] private DateTime _scheduledTime = DateTime.Now;
    [ObservableProperty] private string _status = "待执行";
    [ObservableProperty] private string _remark = string.Empty;
}
