using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class NurseManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Nurse> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private Nurse? _selectedItem;

    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editGender = "女";
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;

    private bool _isNewRecord;

    public NurseManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Nurses.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(n => n.Name.Contains(kw) || n.Department.Contains(kw));
        }
        FilteredItems = new ObservableCollection<Nurse>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditName = string.Empty; EditGender = "女"; EditDepartment = string.Empty; EditPhone = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditName = SelectedItem.Name; EditGender = SelectedItem.Gender;
        EditDepartment = SelectedItem.Department; EditPhone = SelectedItem.Phone;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.Nurses.Add(new Nurse
            {
                Id = _ds.GenerateId(), Name = EditName, Gender = EditGender,
                Department = EditDepartment, Phone = EditPhone
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.Name = EditName; SelectedItem.Gender = EditGender;
            SelectedItem.Department = EditDepartment; SelectedItem.Phone = EditPhone;
        }
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem == null) return;
        _ds.Nurses.Remove(SelectedItem);
        RefreshList();
    }
}
