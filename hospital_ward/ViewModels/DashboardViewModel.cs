using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty] private int _totalBeds;
    [ObservableProperty] private int _availableBeds;
    [ObservableProperty] private int _occupiedBeds;
    [ObservableProperty] private int _maintenanceBeds;
    [ObservableProperty] private int _todayAdmissions;
    [ObservableProperty] private int _todayDischarges;
    [ObservableProperty] private int _inPatientCount;
    [ObservableProperty] private int _doctorCount;
    [ObservableProperty] private int _nurseCount;
    [ObservableProperty] private double _occupancyRate;

    public DashboardViewModel()
    {
        var ds = DataService.Instance;
        TotalBeds = ds.TotalBeds;
        AvailableBeds = ds.AvailableBeds;
        OccupiedBeds = ds.OccupiedBeds;
        MaintenanceBeds = ds.MaintenanceBeds;
        TodayAdmissions = ds.TodayAdmissions;
        TodayDischarges = ds.TodayDischarges;
        InPatientCount = ds.InPatientCount;
        DoctorCount = ds.Doctors.Count;
        NurseCount = ds.Nurses.Count;
        OccupancyRate = TotalBeds > 0 ? System.Math.Round((double)OccupiedBeds / TotalBeds * 100, 1) : 0;
    }
}
