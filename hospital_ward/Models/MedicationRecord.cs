using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFirstApp.Models;

public partial class MedicationRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _medicationName = string.Empty;
    [ObservableProperty] private string _dosage = string.Empty;
    [ObservableProperty] private string _frequency = string.Empty;
    [ObservableProperty] private string _doctor = string.Empty;
    [ObservableProperty] private DateTime _date = DateTime.Now;
}
