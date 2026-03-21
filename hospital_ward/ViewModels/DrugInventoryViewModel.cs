using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DrugInventoryViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<DrugInventoryRecord> _filteredItems = new();
    [ObservableProperty] private DrugInventoryRecord? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editDrugName = string.Empty;
    [ObservableProperty] private string _editBatchNumber = string.Empty;
    [ObservableProperty] private int _editQuantity;
    [ObservableProperty] private decimal _editUnitCost;
    [ObservableProperty] private string _editSupplier = string.Empty;
    [ObservableProperty] private string _editRemark = string.Empty;

    public DrugInventoryViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.DrugInventoryRecords.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.DrugName.Contains(kw) || x.BatchNumber.Contains(kw) || x.Supplier.Contains(kw));
        }

        FilteredItems = new ObservableCollection<DrugInventoryRecord>(query.OrderByDescending(x => x.ReceivedDate));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditDrugName = string.Empty;
        EditBatchNumber = string.Empty;
        EditQuantity = 0;
        EditUnitCost = 0;
        EditSupplier = string.Empty;
        EditRemark = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditDrugName = SelectedItem.DrugName;
        EditBatchNumber = SelectedItem.BatchNumber;
        EditQuantity = SelectedItem.Quantity;
        EditUnitCost = SelectedItem.UnitCost;
        EditSupplier = SelectedItem.Supplier;
        EditRemark = SelectedItem.Remark;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            var catalogItem = _ds.DrugCatalogItems.FirstOrDefault(x => x.Name == EditDrugName);

            _ds.DrugInventoryRecordRepo.Add(new DrugInventoryRecord
            {
                Id = _ds.GenerateId(),
                DrugName = EditDrugName,
                BatchNumber = EditBatchNumber,
                Quantity = EditQuantity,
                UnitCost = EditUnitCost,
                Supplier = EditSupplier,
                ReceivedDate = DateTime.Now,
                Remark = EditRemark
            });

            if (catalogItem != null)
            {
                catalogItem.StockQuantity += EditQuantity;
            }
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.DrugName = EditDrugName;
            SelectedItem.BatchNumber = EditBatchNumber;
            SelectedItem.Quantity = EditQuantity;
            SelectedItem.UnitCost = EditUnitCost;
            SelectedItem.Supplier = EditSupplier;
            SelectedItem.Remark = EditRemark;
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
        _ds.DrugInventoryRecordRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
