using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class Ward : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _wardNumber = string.Empty;
    [ObservableProperty] private string _type = "普通";  // 普通/VIP/ICU
    [ObservableProperty] private int _bedCount;
    [ObservableProperty] private string _department = string.Empty;
}
