using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class NursingManagementViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private readonly UserSessionService _session = UserSessionService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<NursingRecord> _filteredItems = new();
    [ObservableProperty] private NursingRecord? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editNurseName = string.Empty;
    [ObservableProperty] private string _editCareLevel = "一级护理";
    [ObservableProperty] private string _editTaskName = string.Empty;
    [ObservableProperty] private string _editStatus = "待执行";
    [ObservableProperty] private string _editRemark = string.Empty;

    public NursingManagementViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.NursingRecords.AsEnumerable();

        if (_session.CurrentRole == SystemRole.Nurse)
        {
            var nurseName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.NurseName == nurseName);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.PatientName.Contains(kw) || x.NurseName.Contains(kw) || x.Status.Contains(kw));
        }

        FilteredItems = new ObservableCollection<NursingRecord>(query.OrderBy(x => x.ScheduledTime));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty;
        EditNurseName = string.Empty;
        EditCareLevel = "一级护理";
        EditTaskName = string.Empty;
        EditStatus = "待执行";
        EditRemark = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName;
        EditNurseName = SelectedItem.NurseName;
        EditCareLevel = SelectedItem.CareLevel;
        EditTaskName = SelectedItem.TaskName;
        EditStatus = SelectedItem.Status;
        EditRemark = SelectedItem.Remark;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.NursingRecordRepo.Add(new NursingRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                NurseName = EditNurseName,
                CareLevel = EditCareLevel,
                TaskName = EditTaskName,
                ScheduledTime = DateTime.Now,
                Status = EditStatus,
                Remark = EditRemark
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.PatientName = EditPatientName;
            SelectedItem.NurseName = EditNurseName;
            SelectedItem.CareLevel = EditCareLevel;
            SelectedItem.TaskName = EditTaskName;
            SelectedItem.Status = EditStatus;
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
        _ds.NursingRecordRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
