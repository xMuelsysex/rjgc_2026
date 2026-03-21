using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class SystemManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<SystemConfigEntry> _filteredConfigs = new();
    [ObservableProperty] private ObservableCollection<SystemAuditLog> _auditLogs = new();
    [ObservableProperty] private SystemConfigEntry? _selectedConfig;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editCategory = string.Empty;
    [ObservableProperty] private string _editConfigKey = string.Empty;
    [ObservableProperty] private string _editConfigValue = string.Empty;
    [ObservableProperty] private string _editDescription = string.Empty;

    public int TotalConfigs => _ds.SystemConfigEntries.Count;
    public int TotalLogs => _ds.SystemAuditLogs.Count;

    public SystemManagementViewModel()
    {
        RefreshList();
    }

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.SystemConfigEntries.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.Category.Contains(kw) || x.ConfigKey.Contains(kw) || x.ConfigValue.Contains(kw));
        }

        FilteredConfigs = new ObservableCollection<SystemConfigEntry>(query.OrderBy(x => x.Category).ThenBy(x => x.ConfigKey));
        AuditLogs = new ObservableCollection<SystemAuditLog>(_ds.SystemAuditLogs.OrderByDescending(x => x.CreatedAt));
        OnPropertyChanged(nameof(TotalConfigs));
        OnPropertyChanged(nameof(TotalLogs));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditCategory = string.Empty;
        EditConfigKey = string.Empty;
        EditConfigValue = string.Empty;
        EditDescription = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedConfig is null) return;
        _isNewRecord = false;
        EditCategory = SelectedConfig.Category;
        EditConfigKey = SelectedConfig.ConfigKey;
        EditConfigValue = SelectedConfig.ConfigValue;
        EditDescription = SelectedConfig.Description;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.SystemConfigEntryRepo.Add(new SystemConfigEntry
            {
                Id = _ds.GenerateId(),
                Category = EditCategory,
                ConfigKey = EditConfigKey,
                ConfigValue = EditConfigValue,
                Description = EditDescription
            });
            AddAuditLog("新增配置", $"新增 {EditCategory}/{EditConfigKey}");
        }
        else if (SelectedConfig is not null)
        {
            SelectedConfig.Category = EditCategory;
            SelectedConfig.ConfigKey = EditConfigKey;
            SelectedConfig.ConfigValue = EditConfigValue;
            SelectedConfig.Description = EditDescription;
            AddAuditLog("修改配置", $"修改 {EditCategory}/{EditConfigKey}");
        }

        _ds.SaveChanges();
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedConfig is null) return;
        AddAuditLog("删除配置", $"删除 {SelectedConfig.Category}/{SelectedConfig.ConfigKey}");
        _ds.SystemConfigEntryRepo.Remove(SelectedConfig);
        _ds.SaveChanges();
        RefreshList();
    }

    private void AddAuditLog(string action, string detail)
    {
        _ds.SystemAuditLogRepo.Add(new SystemAuditLog
        {
            Id = _ds.GenerateId(),
            OperatorName = "系统管理员",
            Module = "系统管理",
            Action = action,
            CreatedAt = DateTime.Now,
            Detail = detail
        });
    }
}
