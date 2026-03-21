using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DrugCatalogViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<DrugCatalogItem> _filteredItems = new();
    [ObservableProperty] private DrugCatalogItem? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editSpecification = string.Empty;
    [ObservableProperty] private decimal _editUnitPrice;
    [ObservableProperty] private int _editStockQuantity;
    [ObservableProperty] private string _editSupplier = string.Empty;
    [ObservableProperty] private string _editStatus = "正常";

    public DrugCatalogViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.DrugCatalogItems.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.Name.Contains(kw) || x.Supplier.Contains(kw) || x.Status.Contains(kw));
        }

        FilteredItems = new ObservableCollection<DrugCatalogItem>(query.OrderBy(x => x.Name));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditName = string.Empty;
        EditSpecification = string.Empty;
        EditUnitPrice = 0;
        EditStockQuantity = 0;
        EditSupplier = string.Empty;
        EditStatus = "正常";
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditName = SelectedItem.Name;
        EditSpecification = SelectedItem.Specification;
        EditUnitPrice = SelectedItem.UnitPrice;
        EditStockQuantity = SelectedItem.StockQuantity;
        EditSupplier = SelectedItem.Supplier;
        EditStatus = SelectedItem.Status;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.DrugCatalogItemRepo.Add(new DrugCatalogItem
            {
                Id = _ds.GenerateId(),
                Name = EditName,
                Specification = EditSpecification,
                UnitPrice = EditUnitPrice,
                StockQuantity = EditStockQuantity,
                Supplier = EditSupplier,
                ExpiryDate = DateTime.Now.AddYears(1),
                Status = EditStatus
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.Name = EditName;
            SelectedItem.Specification = EditSpecification;
            SelectedItem.UnitPrice = EditUnitPrice;
            SelectedItem.StockQuantity = EditStockQuantity;
            SelectedItem.Supplier = EditSupplier;
            SelectedItem.Status = EditStatus;
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
        _ds.DrugCatalogItemRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
