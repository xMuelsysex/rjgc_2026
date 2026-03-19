using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DischargeViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<DischargeRecord> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private DischargeRecord? _selectedItem;

    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editBedNumber = string.Empty;
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private decimal _editTotalCost;

    private bool _isNewRecord;

    public DischargeViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Discharges.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(d => d.PatientName.Contains(kw) || d.Department.Contains(kw));
        }
        FilteredItems = new ObservableCollection<DischargeRecord>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty; EditBedNumber = string.Empty;
        EditDepartment = string.Empty; EditTotalCost = 0;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName; EditBedNumber = SelectedItem.BedNumber;
        EditDepartment = SelectedItem.Department; EditTotalCost = SelectedItem.TotalCost;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.DischargeRepo.Add(new DischargeRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                BedNumber = EditBedNumber,
                Department = EditDepartment,
                DischargeDate = DateTime.Now,
                TotalCost = EditTotalCost
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.PatientName = EditPatientName; SelectedItem.BedNumber = EditBedNumber;
            SelectedItem.Department = EditDepartment; SelectedItem.TotalCost = EditTotalCost;
        }
        IsEditing = false;
        _ds.SaveChanges();
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.DischargeRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
