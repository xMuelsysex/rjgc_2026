using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFirstApp.Models;

public partial class AdmissionRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _bedNumber = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private string _doctor = string.Empty;
    [ObservableProperty] private DateTime _admissionDate = DateTime.Now;
    [ObservableProperty] private string _diagnosis = string.Empty;
}
