using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class Bed : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _bedNumber = string.Empty;
    [ObservableProperty] private string _wardNumber = string.Empty;
    [ObservableProperty] private string _status = "空闲";  // 空闲/占用/维修
    [ObservableProperty] private string _patientName = string.Empty;
}
