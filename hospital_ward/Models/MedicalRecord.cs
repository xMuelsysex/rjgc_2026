using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFirstApp.Models;

public partial class MedicalRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _doctor = string.Empty;
    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private string _diagnosis = string.Empty;
    [ObservableProperty] private string _treatment = string.Empty;
}
