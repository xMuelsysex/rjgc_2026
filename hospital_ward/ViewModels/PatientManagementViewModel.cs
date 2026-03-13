using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class PatientManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Patient> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private Patient? _selectedItem;

    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editGender = "男";
    [ObservableProperty] private int _editAge;
    [ObservableProperty] private string _editPhone = string.Empty;
    [ObservableProperty] private string _editIdCard = string.Empty;
    [ObservableProperty] private string _editBedNumber = string.Empty;
    [ObservableProperty] private string _editDepartmentName = string.Empty;
    [ObservableProperty] private string _editStatus = "在院";

    private bool _isNewRecord;

    public PatientManagementViewModel()
    {
        RefreshList();
    }

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Patients.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(p => p.Name.Contains(kw) || p.BedNumber.Contains(kw) || p.DepartmentName.Contains(kw));
        }
        FilteredItems = new ObservableCollection<Patient>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditName = string.Empty; EditGender = "男"; EditAge = 0;
        EditPhone = string.Empty; EditIdCard = string.Empty;
        EditBedNumber = string.Empty; EditDepartmentName = string.Empty; EditStatus = "在院";
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditName = SelectedItem.Name; EditGender = SelectedItem.Gender; EditAge = SelectedItem.Age;
        EditPhone = SelectedItem.Phone; EditIdCard = SelectedItem.IdCard;
        EditBedNumber = SelectedItem.BedNumber; EditDepartmentName = SelectedItem.DepartmentName; EditStatus = SelectedItem.Status;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.Patients.Add(new Patient
            {
                Id = _ds.GenerateId(), Name = EditName, Gender = EditGender, Age = EditAge,
                Phone = EditPhone, IdCard = EditIdCard, AdmissionDate = DateTime.Now,
                BedNumber = EditBedNumber, DepartmentName = EditDepartmentName, Status = EditStatus
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.Name = EditName; SelectedItem.Gender = EditGender; SelectedItem.Age = EditAge;
            SelectedItem.Phone = EditPhone; SelectedItem.IdCard = EditIdCard;
            SelectedItem.BedNumber = EditBedNumber; SelectedItem.DepartmentName = EditDepartmentName; SelectedItem.Status = EditStatus;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.Patients.Remove(SelectedItem);
        RefreshList();
    }
}
