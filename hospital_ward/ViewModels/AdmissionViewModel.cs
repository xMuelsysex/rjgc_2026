using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class AdmissionViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<AdmissionRecord> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private AdmissionRecord? _selectedItem;

    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editBedNumber = string.Empty;
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private string _editDoctor = string.Empty;
    [ObservableProperty] private string _editDiagnosis = string.Empty;

    private bool _isNewRecord;

    public AdmissionViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Admissions.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(a => a.PatientName.Contains(kw) || a.Department.Contains(kw) || a.Doctor.Contains(kw));
        }
        FilteredItems = new ObservableCollection<AdmissionRecord>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty; EditBedNumber = string.Empty; EditDepartment = string.Empty;
        EditDoctor = string.Empty; EditDiagnosis = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName; EditBedNumber = SelectedItem.BedNumber;
        EditDepartment = SelectedItem.Department; EditDoctor = SelectedItem.Doctor; EditDiagnosis = SelectedItem.Diagnosis;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.AdmissionRepo.Add(new AdmissionRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                BedNumber = EditBedNumber,
                Department = EditDepartment,
                Doctor = EditDoctor,
                AdmissionDate = DateTime.Now,
                Diagnosis = EditDiagnosis
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.PatientName = EditPatientName; SelectedItem.BedNumber = EditBedNumber;
            SelectedItem.Department = EditDepartment; SelectedItem.Doctor = EditDoctor; SelectedItem.Diagnosis = EditDiagnosis;
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
        _ds.AdmissionRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
