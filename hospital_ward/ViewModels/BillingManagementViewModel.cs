using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class BillingManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private readonly UserSessionService _session = UserSessionService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<BillingRecord> _filteredItems = new();
    [ObservableProperty] private BillingRecord? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private decimal _editTotalAmount;
    [ObservableProperty] private decimal _editPaidAmount;
    [ObservableProperty] private decimal _editInsuranceAmount;
    [ObservableProperty] private string _editStatus = "待结算";

    public BillingManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.BillingRecords.AsEnumerable();

        if (_session.CurrentRole == SystemRole.Patient)
        {
            var patientName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.PatientName == patientName);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.PatientName.Contains(kw) || x.Department.Contains(kw) || x.Status.Contains(kw));
        }

        FilteredItems = new ObservableCollection<BillingRecord>(query.OrderByDescending(x => x.BillingDate));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty;
        EditDepartment = string.Empty;
        EditTotalAmount = 0;
        EditPaidAmount = 0;
        EditInsuranceAmount = 0;
        EditStatus = "待结算";
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName;
        EditDepartment = SelectedItem.Department;
        EditTotalAmount = SelectedItem.TotalAmount;
        EditPaidAmount = SelectedItem.PaidAmount;
        EditInsuranceAmount = SelectedItem.InsuranceAmount;
        EditStatus = SelectedItem.Status;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.BillingRecordRepo.Add(new BillingRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                Department = EditDepartment,
                TotalAmount = EditTotalAmount,
                PaidAmount = EditPaidAmount,
                InsuranceAmount = EditInsuranceAmount,
                Status = EditStatus,
                BillingDate = DateTime.Now
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.PatientName = EditPatientName;
            SelectedItem.Department = EditDepartment;
            SelectedItem.TotalAmount = EditTotalAmount;
            SelectedItem.PaidAmount = EditPaidAmount;
            SelectedItem.InsuranceAmount = EditInsuranceAmount;
            SelectedItem.Status = EditStatus;
        }

        _ds.SaveChanges();
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem is null) return;
        _ds.BillingRecordRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
