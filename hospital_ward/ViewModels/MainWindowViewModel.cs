using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyFirstApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<NavigationItem> MenuItems { get; } = new();

    [ObservableProperty]
    private ViewModelBase _currentPage = null!;

    [ObservableProperty]
    private NavigationItem _selectedMenuItem = null!;

    public MainWindowViewModel()
    {
        MenuItems.Add(new NavigationItem("系统首页", "Home", typeof(DashboardViewModel)));
        MenuItems.Add(new NavigationItem("科室管理", "Department", typeof(DepartmentManagementViewModel)));
        MenuItems.Add(new NavigationItem("病房管理", "Ward", typeof(WardManagementViewModel)));
        MenuItems.Add(new NavigationItem("病房床位", "Bed", typeof(BedManagementViewModel)));
        MenuItems.Add(new NavigationItem("患者管理", "Person", typeof(PatientManagementViewModel)));
        MenuItems.Add(new NavigationItem("医生管理", "Doctor", typeof(DoctorManagementViewModel)));
        MenuItems.Add(new NavigationItem("护士管理", "Nurse", typeof(NurseManagementViewModel)));
        MenuItems.Add(new NavigationItem("入院登记", "Admission", typeof(AdmissionViewModel)));
        MenuItems.Add(new NavigationItem("出院登记", "Discharge", typeof(DischargeViewModel)));
        MenuItems.Add(new NavigationItem("诊疗信息", "Medical", typeof(MedicalRecordViewModel)));
        MenuItems.Add(new NavigationItem("用药信息", "Medication", typeof(MedicationViewModel)));

        // Default page
        SelectedMenuItem = MenuItems[0];
        NavigateTo(SelectedMenuItem);
    }

    partial void OnSelectedMenuItemChanged(NavigationItem value)
    {
        if (value != null)
        {
            NavigateTo(value);
        }
    }

    private void NavigateTo(NavigationItem item)
    {
        CurrentPage = item.ViewModelType.Name switch
        {
            nameof(DashboardViewModel) => new DashboardViewModel(),
            nameof(BedManagementViewModel) => new BedManagementViewModel(),
            nameof(PatientManagementViewModel) => new PatientManagementViewModel(),
            nameof(DoctorManagementViewModel) => new DoctorManagementViewModel(),
            nameof(NurseManagementViewModel) => new NurseManagementViewModel(),
            nameof(DepartmentManagementViewModel) => new DepartmentManagementViewModel(),
            nameof(WardManagementViewModel) => new WardManagementViewModel(),
            nameof(AdmissionViewModel) => new AdmissionViewModel(),
            nameof(DischargeViewModel) => new DischargeViewModel(),
            nameof(MedicalRecordViewModel) => new MedicalRecordViewModel(),
            nameof(MedicationViewModel) => new MedicationViewModel(),
            _ => new DashboardViewModel()
        };
    }
}

public record NavigationItem(string Label, string IconKey, System.Type ViewModelType);
