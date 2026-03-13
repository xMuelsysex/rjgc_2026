using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DoctorManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Doctor> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private Doctor? _selectedItem;

    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editGender = "男";
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private string _editTitle = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;

    private bool _isNewRecord;

    public DoctorManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Doctors.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(d => d.Name.Contains(kw) || d.Department.Contains(kw) || d.Title.Contains(kw));
        }
        FilteredItems = new ObservableCollection<Doctor>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditName = string.Empty; EditGender = "男"; EditDepartment = string.Empty;
        EditTitle = string.Empty; EditPhone = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditName = SelectedItem.Name; EditGender = SelectedItem.Gender;
        EditDepartment = SelectedItem.Department; EditTitle = SelectedItem.Title; EditPhone = SelectedItem.Phone;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.Doctors.Add(new Doctor
            {
                Id = _ds.GenerateId(), Name = EditName, Gender = EditGender,
                Department = EditDepartment, Title = EditTitle, Phone = EditPhone
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.Name = EditName; SelectedItem.Gender = EditGender;
            SelectedItem.Department = EditDepartment; SelectedItem.Title = EditTitle; SelectedItem.Phone = EditPhone;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.Doctors.Remove(SelectedItem);
        RefreshList();
    }
}
