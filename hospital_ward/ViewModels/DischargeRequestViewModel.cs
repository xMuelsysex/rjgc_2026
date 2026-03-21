using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DischargeRequestViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private readonly UserSessionService _session = UserSessionService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<DischargeRequest> _filteredItems = new();
    [ObservableProperty] private DischargeRequest? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editPatientName = string.Empty;
    [ObservableProperty] private string _editDepartment = string.Empty;
    [ObservableProperty] private string _editAttendingDoctor = string.Empty;
    [ObservableProperty] private string _editStatus = "待审核";
    [ObservableProperty] private string _editReason = string.Empty;
    [ObservableProperty] private string _editReviewComment = string.Empty;

    public DischargeRequestViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.DischargeRequests.AsEnumerable();

        if (_session.CurrentRole == SystemRole.Patient)
        {
            var patientName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.PatientName == patientName);
        }
        else if (_session.CurrentRole == SystemRole.Doctor)
        {
            var doctorName = _session.CurrentProfile?.DisplayName;
            query = query.Where(x => x.AttendingDoctor == doctorName);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.PatientName.Contains(kw) || x.Department.Contains(kw) || x.Status.Contains(kw));
        }

        FilteredItems = new ObservableCollection<DischargeRequest>(query.OrderByDescending(x => x.RequestedDate));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditPatientName = string.Empty;
        EditDepartment = string.Empty;
        EditAttendingDoctor = string.Empty;
        EditStatus = "待审核";
        EditReason = string.Empty;
        EditReviewComment = string.Empty;
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditPatientName = SelectedItem.PatientName;
        EditDepartment = SelectedItem.Department;
        EditAttendingDoctor = SelectedItem.AttendingDoctor;
        EditStatus = SelectedItem.Status;
        EditReason = SelectedItem.Reason;
        EditReviewComment = SelectedItem.ReviewComment;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.DischargeRequestRepo.Add(new DischargeRequest
            {
                Id = _ds.GenerateId(),
                PatientName = EditPatientName,
                Department = EditDepartment,
                AttendingDoctor = EditAttendingDoctor,
                RequestedDate = DateTime.Now,
                Status = EditStatus,
                Reason = EditReason,
                ReviewComment = EditReviewComment
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.PatientName = EditPatientName;
            SelectedItem.Department = EditDepartment;
            SelectedItem.AttendingDoctor = EditAttendingDoctor;
            SelectedItem.Status = EditStatus;
            SelectedItem.Reason = EditReason;
            SelectedItem.ReviewComment = EditReviewComment;
        }

        if (EditStatus == "已批准")
        {
            var billing = _ds.BillingRecords.FirstOrDefault(x => x.PatientName == EditPatientName);
            if (billing == null)
            {
                _ds.BillingRecordRepo.Add(new BillingRecord
                {
                    Id = _ds.GenerateId(),
                    PatientName = EditPatientName,
                    Department = EditDepartment,
                    TotalAmount = 5000m,
                    PaidAmount = 0m,
                    InsuranceAmount = 0m,
                    Status = "待结算",
                    BillingDate = DateTime.Now
                });
            }
            else
            {
                billing.Status = "待结算";
                billing.BillingDate = DateTime.Now;
            }
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
        _ds.DischargeRequestRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
