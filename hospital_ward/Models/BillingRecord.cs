using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class BillingRecord : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _patientName = string.Empty;
    [ObservableProperty] private string _department = string.Empty;
    [ObservableProperty] private decimal _totalAmount;
    [ObservableProperty] private decimal _paidAmount;
    [ObservableProperty] private decimal _insuranceAmount;
    [ObservableProperty] private string _status = "待结算";
    [ObservableProperty] private DateTime _billingDate = DateTime.Now;

    public decimal OutstandingAmount => TotalAmount - PaidAmount - InsuranceAmount;
}
