using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DepartmentManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Department> _filteredItems = new();
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private Department? _selectedItem;

    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private string _editDirector = string.Empty;

    private bool _isNewRecord;

    public DepartmentManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.Departments.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(d => d.Name.Contains(kw) || d.Director.Contains(kw));
        }
        FilteredItems = new ObservableCollection<Department>(query);
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditName = string.Empty; EditDescription = string.Empty; EditDirector = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem == null) return;
        _isNewRecord = false;
        EditName = SelectedItem.Name; EditDescription = SelectedItem.Description; EditDirector = SelectedItem.Director;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.DepartmentRepo.Add(new Department
            {
                Id = _ds.GenerateId(),
                Name = EditName,
                Description = EditDescription,
                Director = EditDirector
            });
        }
        else if (SelectedItem != null)
        {
            SelectedItem.Name = EditName; SelectedItem.Description = EditDescription; SelectedItem.Director = EditDirector;
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
        _ds.DepartmentRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
