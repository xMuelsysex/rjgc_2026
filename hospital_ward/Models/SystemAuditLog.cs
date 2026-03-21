using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class SystemAuditLog : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _operatorName = string.Empty;
    [ObservableProperty] private string _module = string.Empty;
    [ObservableProperty] private string _action = string.Empty;
    [ObservableProperty] private DateTime _createdAt = DateTime.Now;
    [ObservableProperty] private string _detail = string.Empty;
}
