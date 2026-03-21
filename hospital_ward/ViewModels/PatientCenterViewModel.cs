using System.Linq;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public class PatientCenterViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private Patient? CurrentPatient => _ds.Patients.FirstOrDefault();
    private Doctor? CurrentDoctor => _ds.Doctors.FirstOrDefault(d => d.Department == CurrentPatient?.DepartmentName);
    private Nurse? CurrentNurse => _ds.Nurses.FirstOrDefault(n => n.Department == CurrentPatient?.DepartmentName);

    public string PatientName => CurrentPatient?.Name ?? "未分配";
    public string PatientStatus => CurrentPatient?.Status ?? "未知";
    public string BedNumber => CurrentPatient?.BedNumber ?? "未分配";
    public string DepartmentName => CurrentPatient?.DepartmentName ?? "未分配";
    public string DoctorName => CurrentDoctor?.Name ?? "未分配";
    public string NurseName => CurrentNurse?.Name ?? "未分配";
}
