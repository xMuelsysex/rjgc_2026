using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class MedicalDeviceViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<MedicalDevice> _filteredItems = new();
    [ObservableProperty] private MedicalDevice? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editDeviceName = string.Empty;
    [ObservableProperty] private string _editModelNumber = string.Empty;
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private string _editManager = string.Empty;
    [ObservableProperty] private string _editStatus = "在用";
    [ObservableProperty] private string _editMaintenanceCycle = string.Empty;

    public MedicalDeviceViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.MedicalDevices.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.DeviceName.Contains(kw) || x.Department.Contains(kw) || x.Status.Contains(kw));
        }

        FilteredItems = new ObservableCollection<MedicalDevice>(query.OrderBy(x => x.DeviceName));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditDeviceName = string.Empty;
        EditModelNumber = string.Empty;
        EditDepartment = string.Empty;
        EditManager = string.Empty;
        EditStatus = "在用";
        EditMaintenanceCycle = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditDeviceName = SelectedItem.DeviceName;
        EditModelNumber = SelectedItem.ModelNumber;
        EditDepartment = SelectedItem.Department;
        EditManager = SelectedItem.Manager;
        EditStatus = SelectedItem.Status;
        EditMaintenanceCycle = SelectedItem.MaintenanceCycle;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.MedicalDeviceRepo.Add(new MedicalDevice
            {
                Id = _ds.GenerateId(),
                DeviceName = EditDeviceName,
                ModelNumber = EditModelNumber,
                Department = EditDepartment,
                Manager = EditManager,
                PurchaseDate = DateTime.Now,
                Status = EditStatus,
                MaintenanceCycle = EditMaintenanceCycle
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.DeviceName = EditDeviceName;
            SelectedItem.ModelNumber = EditModelNumber;
            SelectedItem.Department = EditDepartment;
            SelectedItem.Manager = EditManager;
            SelectedItem.Status = EditStatus;
            SelectedItem.MaintenanceCycle = EditMaintenanceCycle;
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
        _ds.MedicalDeviceRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
