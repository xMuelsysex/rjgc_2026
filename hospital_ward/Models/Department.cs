using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class Department : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _director = string.Empty;
}
