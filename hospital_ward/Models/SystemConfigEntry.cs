using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class SystemConfigEntry : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _category = string.Empty;
    [ObservableProperty] private string _configKey = string.Empty;
    [ObservableProperty] private string _configValue = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
}
