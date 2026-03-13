using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class BedManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Bed> _filteredBeds = new();

    // 编辑表单字段
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editBedNumber = string.Empty;
    [ObservableProperty] private string _editWardNumber = string.Empty;
    [ObservableProperty] private string _editStatus = "空闲";
    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private Bed? _selectedBed;

    private bool _isNewRecord;

    public BedManagementViewModel()
    {
        RefreshList();
    }

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Beds.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(b => b.BedNumber.Contains(kw) || b.WardNumber.Contains(kw) || b.PatientName.Contains(kw));
        }
        FilteredBeds = new ObservableCollection<Bed>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditBedNumber = string.Empty;
        EditWardNumber = string.Empty;
        EditStatus = "空闲";
        EditPatientName = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedBed == null) return;
        _isNewRecord = false;
        EditBedNumber = SelectedBed.BedNumber;
        EditWardNumber = SelectedBed.WardNumber;
        EditStatus = SelectedBed.Status;
        EditPatientName = SelectedBed.PatientName;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.Beds.Add(new Bed
            {
                Id = _ds.GenerateId(),
                BedNumber = EditBedNumber,
                WardNumber = EditWardNumber,
                Status = EditStatus,
                PatientName = EditPatientName
            });
        }
        else if (SelectedBed != null)
        {
            SelectedBed.BedNumber = EditBedNumber;
            SelectedBed.WardNumber = EditWardNumber;
            SelectedBed.Status = EditStatus;
            SelectedBed.PatientName = EditPatientName;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand]
    private void Cancel()
    {
        IsEditing = false;
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedBed == null) return;
        _ds.Beds.Remove(SelectedBed);
        RefreshList();
    }
}
