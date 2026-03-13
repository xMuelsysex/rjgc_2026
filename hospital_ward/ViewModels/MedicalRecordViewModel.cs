using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class MedicalRecordViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<MedicalRecord> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private MedicalRecord? _selectedItem;

    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editDoctor = string.Empty;
    [ObservableProperty] private string _editDiagnosis = string.Empty;
    [ObservableProperty] private string _editTreatment = string.Empty;

    private bool _isNewRecord;

    public MedicalRecordViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.MedicalRecords.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(m => m.PatientName.Contains(kw) || m.Doctor.Contains(kw) || m.Diagnosis.Contains(kw));
        }
        FilteredItems = new ObservableCollection<MedicalRecord>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty; EditDoctor = string.Empty;
        EditDiagnosis = string.Empty; EditTreatment = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName; EditDoctor = SelectedItem.Doctor;
        EditDiagnosis = SelectedItem.Diagnosis; EditTreatment = SelectedItem.Treatment;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.MedicalRecords.Add(new MedicalRecord
            {
                Id = _ds.GenerateId(), PatientName = EditPatientName, Doctor = EditDoctor,
                Date = DateTime.Now, Diagnosis = EditDiagnosis, Treatment = EditTreatment
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.PatientName = EditPatientName; SelectedItem.Doctor = EditDoctor;
            SelectedItem.Diagnosis = EditDiagnosis; SelectedItem.Treatment = EditTreatment;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.MedicalRecords.Remove(SelectedItem);
        RefreshList();
    }
}
