using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class DischargeRequest : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _attendingDoctor = string.Empty;
    [ObservableProperty] private DateTime _requestedDate = DateTime.Now;
    [ObservableProperty] private string _status = "待审核";
    [ObservableProperty] private string _reason = string.Empty;
    [ObservableProperty] private string _reviewComment = string.Empty;
}
