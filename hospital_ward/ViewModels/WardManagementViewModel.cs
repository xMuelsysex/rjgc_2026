using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class WardManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Ward> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private Ward? _selectedItem;

    [ObservableProperty] private string _editWardNumber = string.Empty;
    [ObservableProperty] private string _editType = "普通";
    [ObservableProperty] private int _editBedCount;
    [ObservableProperty] private string _editDepartment = string.Empty;

    private bool _isNewRecord;

    public WardManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Wards.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(w => w.WardNumber.Contains(kw) || w.Department.Contains(kw) || w.Type.Contains(kw));
        }
        FilteredItems = new ObservableCollection<Ward>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditWardNumber = string.Empty; EditType = "普通"; EditBedCount = 0; EditDepartment = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditWardNumber = SelectedItem.WardNumber; EditType = SelectedItem.Type;
        EditBedCount = SelectedItem.BedCount; EditDepartment = SelectedItem.Department;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.Wards.Add(new Ward
            {
                Id = _ds.GenerateId(), WardNumber = EditWardNumber, Type = EditType,
                BedCount = EditBedCount, Department = EditDepartment
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.WardNumber = EditWardNumber; SelectedItem.Type = EditType;
            SelectedItem.BedCount = EditBedCount; SelectedItem.Department = EditDepartment;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.Wards.Remove(SelectedItem);
        RefreshList();
    }
}
