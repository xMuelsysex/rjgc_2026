using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class DrugInventoryRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _drugName = string.Empty;
    [ObservableProperty] private string _batchNumber = string.Empty;
    [ObservableProperty] private int _quantity;
    [ObservableProperty] private decimal _unitCost;
    [ObservableProperty] private string _supplier = string.Empty;
    [ObservableProperty] private DateTime _receivedDate = DateTime.Now;
    [ObservableProperty] private string _remark = string.Empty;
}
