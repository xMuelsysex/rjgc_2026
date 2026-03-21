using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class DrugCatalogItem : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _specification = string.Empty;
    [ObservableProperty] private decimal _unitPrice;
    [ObservableProperty] private int _stockQuantity;
    [ObservableProperty] private string _supplier = string.Empty;
    [ObservableProperty] private DateTime _expiryDate = DateTime.Now.AddMonths(12);
    [ObservableProperty] private string _status = "正常";
}
