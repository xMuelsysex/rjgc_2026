using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class PatientOrderViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private readonly UserSessionService _session = UserSessionService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<PatientOrder> _filteredItems = new();
    [ObservableProperty] private PatientOrder? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editOrderType = "长期医嘱";
    [ObservableProperty] private string _editContent = string.Empty;
    [ObservableProperty] private string _editDoctorName = string.Empty;
    [ObservableProperty] private string _editExecutionStatus = "待执行";
    [ObservableProperty] private string _editNotes = string.Empty;

    public PatientOrderViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.PatientOrders.AsEnumerable();

        if (_session.CurrentRole == SystemRole.Patient)
        {
            var patientName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.PatientName == patientName);
        }
        else if (_session.CurrentRole == SystemRole.Doctor)
        {
            var doctorName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.DoctorName == doctorName);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.PatientName.Contains(kw) || x.Content.Contains(kw) || x.ExecutionStatus.Contains(kw));
        }

        FilteredItems = new ObservableCollection<PatientOrder>(query.OrderByDescending(x => x.StartDate));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty;
        EditOrderType = "长期医嘱";
        EditContent = string.Empty;
        EditDoctorName = string.Empty;
        EditExecutionStatus = "待执行";
        EditNotes = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName;
        EditOrderType = SelectedItem.OrderType;
        EditContent = SelectedItem.Content;
        EditDoctorName = SelectedItem.DoctorName;
        EditExecutionStatus = SelectedItem.ExecutionStatus;
        EditNotes = SelectedItem.Notes;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.PatientOrderRepo.Add(new PatientOrder
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                OrderType = EditOrderType,
                Content = EditContent,
                DoctorName = EditDoctorName,
                ExecutionStatus = EditExecutionStatus,
                StartDate = DateTime.Now,
                Notes = EditNotes
            });

            _ds.NursingRecordRepo.Add(new NursingRecord
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                NurseName = _ds.Nurses.FirstOrDefault(n => n.Department == _ds.Patients.FirstOrDefault(p => p.Name == EditPatientName)?.DepartmentName)?.Name ?? "待分配",
                CareLevel = "一级护理",
                TaskName = $"执行医嘱：{EditContent}",
                ScheduledTime = DateTime.Now,
                Status = "待执行",
                Remark = EditNotes
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.PatientName = EditPatientName;
            SelectedItem.OrderType = EditOrderType;
            SelectedItem.Content = EditContent;
            SelectedItem.DoctorName = EditDoctorName;
            SelectedItem.ExecutionStatus = EditExecutionStatus;
            SelectedItem.Notes = EditNotes;
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
        _ds.PatientOrderRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
