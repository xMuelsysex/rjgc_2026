using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Data;
using MyFirstApp.Models;

namespace MyFirstApp.Services;

public class DataService
{
    private static readonly Lazy<DataService> _instance = new(() => new DataService());
    public static DataService Instance => _instance.Value;

    private readonly HospitalDbContext _context;

    // 仓储 —— 未来新增模型只需在此添加一行 Repository 属性
    public IRepository<Patient> PatientRepo { get; }
    public IRepository<Doctor> DoctorRepo { get; }
    public IRepository<Nurse> NurseRepo { get; }
    public IRepository<Department> DepartmentRepo { get; }
    public IRepository<Ward> WardRepo { get; }
    public IRepository<Bed> BedRepo { get; }
    public IRepository<AdmissionRecord> AdmissionRepo { get; }
    public IRepository<DischargeRecord> DischargeRepo { get; }
    public IRepository<MedicalRecord> MedicalRecordRepo { get; }
    public IRepository<MedicationRecord> MedicationRecordRepo { get; }

    // 向后兼容的快捷属性 —— ViewModel 可继续使用 _ds.Patients 等
    public ObservableCollection<Patient> Patients => PatientRepo.Items;
    public ObservableCollection<Doctor> Doctors => DoctorRepo.Items;
    public ObservableCollection<Nurse> Nurses => NurseRepo.Items;
    public ObservableCollection<Department> Departments => DepartmentRepo.Items;
    public ObservableCollection<Ward> Wards => WardRepo.Items;
    public ObservableCollection<Bed> Beds => BedRepo.Items;
    public ObservableCollection<AdmissionRecord> Admissions => AdmissionRepo.Items;
    public ObservableCollection<DischargeRecord> Discharges => DischargeRepo.Items;
    public ObservableCollection<MedicalRecord> MedicalRecords => MedicalRecordRepo.Items;
    public ObservableCollection<MedicationRecord> MedicationRecords => MedicationRecordRepo.Items;

    private DataService()
    {
        _context = new HospitalDbContext();

        // 确保数据库已创建（含种子数据）
        _context.Database.EnsureCreated();

        // 初始化所有仓储
        PatientRepo = new DbRepository<Patient>(_context);
        DoctorRepo = new DbRepository<Doctor>(_context);
        NurseRepo = new DbRepository<Nurse>(_context);
        DepartmentRepo = new DbRepository<Department>(_context);
        WardRepo = new DbRepository<Ward>(_context);
        BedRepo = new DbRepository<Bed>(_context);
        AdmissionRepo = new DbRepository<AdmissionRecord>(_context);
        DischargeRepo = new DbRepository<DischargeRecord>(_context);
        MedicalRecordRepo = new DbRepository<MedicalRecord>(_context);
        MedicationRecordRepo = new DbRepository<MedicationRecord>(_context);

        // 从数据库加载数据
        LoadAll();
    }

    private void LoadAll()
    {
        PatientRepo.Load();
        DoctorRepo.Load();
        NurseRepo.Load();
        DepartmentRepo.Load();
        WardRepo.Load();
        BedRepo.Load();
        AdmissionRepo.Load();
        DischargeRepo.Load();
        MedicalRecordRepo.Load();
        MedicationRecordRepo.Load();
    }

    /// <summary>
    /// 将所有更改持久化到数据库。
    /// ViewModel 在 Add/Edit/Delete 操作后应调用此方法。
    /// </summary>
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public int GenerateId() => 0; // EF Core 使用自增 Id，传 0 即可

    // 统计方法
    public int TotalBeds => Beds.Count;
    public int AvailableBeds => Beds.Count(b => b.Status == "空闲");
    public int OccupiedBeds => Beds.Count(b => b.Status == "占用");
    public int MaintenanceBeds => Beds.Count(b => b.Status == "维修");
    public int TodayAdmissions => Admissions.Count(a => a.AdmissionDate.Date == DateTime.Today);
    public int TodayDischarges => Discharges.Count(d => d.DischargeDate.Date == DateTime.Today);
    public int InPatientCount => Patients.Count(p => p.Status == "在院");
}
