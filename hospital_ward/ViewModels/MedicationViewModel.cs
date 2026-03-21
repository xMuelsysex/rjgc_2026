using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class MedicationViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private readonly UserSessionService _session = UserSessionService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<MedicationRecord> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private MedicationRecord? _selectedItem;

    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editMedicationName = string.Empty;
    [ObservableProperty] private string _editDosage = string.Empty;
    [ObservableProperty] private string _editFrequency = string.Empty;
    [ObservableProperty] private string _editDoctor = string.Empty;

    private bool _isNewRecord;

    public MedicationViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.MedicationRecords.AsEnumerable();

        if (_session.CurrentRole == SystemRole.Patient)
        {
            var patientName = _session.CurrentProfile?.DisplayName;
            query = query.Where(m => m.PatientName == patientName);
        }
        else if (_session.CurrentRole == SystemRole.Doctor)
        {
            var doctorName = _session.CurrentProfile?.DisplayName;
            query = query.Where(m => m.Doctor == doctorName);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(m => m.PatientName.Contains(kw) || m.MedicationName.Contains(kw) || m.Doctor.Contains(kw));
        }
        FilteredItems = new ObservableCollection<MedicationRecord>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty; EditMedicationName = string.Empty;
        EditDosage = string.Empty; EditFrequency = string.Empty; EditDoctor = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName; EditMedicationName = SelectedItem.MedicationName;
        EditDosage = SelectedItem.Dosage; EditFrequency = SelectedItem.Frequency; EditDoctor = SelectedItem.Doctor;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.MedicationRecordRepo.Add(new MedicationRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                MedicationName = EditMedicationName,
                Dosage = EditDosage,
                Frequency = EditFrequency,
                Doctor = EditDoctor,
                Date = DateTime.Now
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.PatientName = EditPatientName; SelectedItem.MedicationName = EditMedicationName;
            SelectedItem.Dosage = EditDosage; SelectedItem.Frequency = EditFrequency; SelectedItem.Doctor = EditDoctor;
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
        _ds.MedicationRecordRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
