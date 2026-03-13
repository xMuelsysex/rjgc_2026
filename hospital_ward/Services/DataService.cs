using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyFirstApp.Models;

namespace MyFirstApp.Services;

public class DataService
{
    private static readonly Lazy<DataService> _instance = new(() => new DataService());
    public static DataService Instance => _instance.Value;

    private int _nextId = 100;
    private int NextId() => _nextId++;

    public ObservableCollection<Patient> Patients { get; } = new();
    public ObservableCollection<Doctor> Doctors { get; } = new();
    public ObservableCollection<Nurse> Nurses { get; } = new();
    public ObservableCollection<Department> Departments { get; } = new();
    public ObservableCollection<Ward> Wards { get; } = new();
    public ObservableCollection<Bed> Beds { get; } = new();
    public ObservableCollection<AdmissionRecord> Admissions { get; } = new();
    public ObservableCollection<DischargeRecord> Discharges { get; } = new();
    public ObservableCollection<MedicalRecord> MedicalRecords { get; } = new();
    public ObservableCollection<MedicationRecord> MedicationRecords { get; } = new();

    private DataService()
    {
        SeedData();
    }

    private void SeedData()
    {
        // 科室
        Departments.Add(new Department { Id = NextId(), Name = "内科", Description = "负责内科疾病诊疗", Director = "王主任" });
        Departments.Add(new Department { Id = NextId(), Name = "外科", Description = "负责外科手术及术后恢复", Director = "李主任" });
        Departments.Add(new Department { Id = NextId(), Name = "儿科", Description = "负责儿童疾病诊疗", Director = "赵主任" });
        Departments.Add(new Department { Id = NextId(), Name = "妇产科", Description = "负责妇产科疾病诊疗", Director = "孙主任" });
        Departments.Add(new Department { Id = NextId(), Name = "急诊科", Description = "负责急诊抢救", Director = "周主任" });

        // 医生
        Doctors.Add(new Doctor { Id = NextId(), Name = "张伟", Gender = "男", Department = "内科", Title = "主任医师", Phone = "13800000001" });
        Doctors.Add(new Doctor { Id = NextId(), Name = "刘洋", Gender = "女", Department = "外科", Title = "副主任医师", Phone = "13800000002" });
        Doctors.Add(new Doctor { Id = NextId(), Name = "陈静", Gender = "女", Department = "儿科", Title = "主治医师", Phone = "13800000003" });
        Doctors.Add(new Doctor { Id = NextId(), Name = "王强", Gender = "男", Department = "妇产科", Title = "主治医师", Phone = "13800000004" });
        Doctors.Add(new Doctor { Id = NextId(), Name = "赵敏", Gender = "女", Department = "急诊科", Title = "住院医师", Phone = "13800000005" });

        // 护士
        Nurses.Add(new Nurse { Id = NextId(), Name = "林小红", Gender = "女", Department = "内科", Phone = "13900000001" });
        Nurses.Add(new Nurse { Id = NextId(), Name = "黄丽", Gender = "女", Department = "外科", Phone = "13900000002" });
        Nurses.Add(new Nurse { Id = NextId(), Name = "吴芳", Gender = "女", Department = "儿科", Phone = "13900000003" });
        Nurses.Add(new Nurse { Id = NextId(), Name = "郑婷", Gender = "女", Department = "急诊科", Phone = "13900000004" });

        // 病房
        Wards.Add(new Ward { Id = NextId(), WardNumber = "A区-101", Type = "普通", BedCount = 4, Department = "内科" });
        Wards.Add(new Ward { Id = NextId(), WardNumber = "A区-102", Type = "普通", BedCount = 4, Department = "内科" });
        Wards.Add(new Ward { Id = NextId(), WardNumber = "B区-201", Type = "VIP", BedCount = 2, Department = "外科" });
        Wards.Add(new Ward { Id = NextId(), WardNumber = "B区-202", Type = "普通", BedCount = 4, Department = "外科" });
        Wards.Add(new Ward { Id = NextId(), WardNumber = "C区-301", Type = "ICU", BedCount = 2, Department = "急诊科" });
        Wards.Add(new Ward { Id = NextId(), WardNumber = "D区-401", Type = "普通", BedCount = 3, Department = "儿科" });

        // 床位
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A101-01", WardNumber = "A区-101", Status = "占用", PatientName = "张三" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A101-02", WardNumber = "A区-101", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A101-03", WardNumber = "A区-101", Status = "占用", PatientName = "王五" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A101-04", WardNumber = "A区-101", Status = "维修", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A102-01", WardNumber = "A区-102", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "A102-02", WardNumber = "A区-102", Status = "占用", PatientName = "赵六" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "B201-01", WardNumber = "B区-201", Status = "占用", PatientName = "李四" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "B201-02", WardNumber = "B区-201", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "B202-01", WardNumber = "B区-202", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "B202-02", WardNumber = "B区-202", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "C301-01", WardNumber = "C区-301", Status = "占用", PatientName = "孙七" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "C301-02", WardNumber = "C区-301", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "D401-01", WardNumber = "D区-401", Status = "空闲", PatientName = "" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "D401-02", WardNumber = "D区-401", Status = "占用", PatientName = "小明" });
        Beds.Add(new Bed { Id = NextId(), BedNumber = "D401-03", WardNumber = "D区-401", Status = "空闲", PatientName = "" });

        // 患者
        Patients.Add(new Patient { Id = NextId(), Name = "张三", Gender = "男", Age = 45, Phone = "13700000001", IdCard = "110101198001010011", AdmissionDate = DateTime.Now.AddDays(-5), BedNumber = "A101-01", DepartmentName = "内科", Status = "在院" });
        Patients.Add(new Patient { Id = NextId(), Name = "李四", Gender = "男", Age = 60, Phone = "13700000002", IdCard = "110101196501020022", AdmissionDate = DateTime.Now.AddDays(-3), BedNumber = "B201-01", DepartmentName = "外科", Status = "在院" });
        Patients.Add(new Patient { Id = NextId(), Name = "王五", Gender = "女", Age = 32, Phone = "13700000003", IdCard = "110101199301030033", AdmissionDate = DateTime.Now.AddDays(-1), BedNumber = "A101-03", DepartmentName = "内科", Status = "在院" });
        Patients.Add(new Patient { Id = NextId(), Name = "赵六", Gender = "男", Age = 28, Phone = "13700000004", IdCard = "110101199701040044", AdmissionDate = DateTime.Now.AddDays(-2), BedNumber = "A102-02", DepartmentName = "内科", Status = "在院" });
        Patients.Add(new Patient { Id = NextId(), Name = "孙七", Gender = "女", Age = 55, Phone = "13700000005", IdCard = "110101197001050055", AdmissionDate = DateTime.Now.AddDays(-7), BedNumber = "C301-01", DepartmentName = "急诊科", Status = "在院" });
        Patients.Add(new Patient { Id = NextId(), Name = "小明", Gender = "男", Age = 8, Phone = "13700000006", IdCard = "110101201801060066", AdmissionDate = DateTime.Now, BedNumber = "D401-02", DepartmentName = "儿科", Status = "在院" });

        // 入院登记
        foreach (var p in Patients)
        {
            Admissions.Add(new AdmissionRecord
            {
                Id = NextId(),
                PatientName = p.Name,
                BedNumber = p.BedNumber,
                Department = p.DepartmentName,
                Doctor = Doctors.FirstOrDefault(d => d.Department == p.DepartmentName)?.Name ?? "未分配",
                AdmissionDate = p.AdmissionDate,
                Diagnosis = "待诊断"
            });
        }

        // 出院登记
        Discharges.Add(new DischargeRecord { Id = NextId(), PatientName = "周八", BedNumber = "A102-01", Department = "内科", DischargeDate = DateTime.Now.AddDays(-1), TotalCost = 5680.50m });
        Discharges.Add(new DischargeRecord { Id = NextId(), PatientName = "吴九", BedNumber = "B202-01", Department = "外科", DischargeDate = DateTime.Now.AddDays(-2), TotalCost = 12350.00m });

        // 诊疗信息
        MedicalRecords.Add(new MedicalRecord { Id = NextId(), PatientName = "张三", Doctor = "张伟", Date = DateTime.Now.AddDays(-4), Diagnosis = "高血压", Treatment = "降压药物治疗" });
        MedicalRecords.Add(new MedicalRecord { Id = NextId(), PatientName = "李四", Doctor = "刘洋", Date = DateTime.Now.AddDays(-2), Diagnosis = "骨折", Treatment = "手术复位固定" });
        MedicalRecords.Add(new MedicalRecord { Id = NextId(), PatientName = "孙七", Doctor = "赵敏", Date = DateTime.Now.AddDays(-6), Diagnosis = "急性胃炎", Treatment = "输液+药物治疗" });
        MedicalRecords.Add(new MedicalRecord { Id = NextId(), PatientName = "小明", Doctor = "陈静", Date = DateTime.Now, Diagnosis = "支气管炎", Treatment = "雾化+抗生素治疗" });

        // 用药信息
        MedicationRecords.Add(new MedicationRecord { Id = NextId(), PatientName = "张三", MedicationName = "硝苯地平", Dosage = "10mg", Frequency = "每日两次", Doctor = "张伟", Date = DateTime.Now.AddDays(-4) });
        MedicationRecords.Add(new MedicationRecord { Id = NextId(), PatientName = "李四", MedicationName = "布洛芬", Dosage = "200mg", Frequency = "每日三次", Doctor = "刘洋", Date = DateTime.Now.AddDays(-2) });
        MedicationRecords.Add(new MedicationRecord { Id = NextId(), PatientName = "孙七", MedicationName = "奥美拉唑", Dosage = "20mg", Frequency = "每日一次", Doctor = "赵敏", Date = DateTime.Now.AddDays(-6) });
        MedicationRecords.Add(new MedicationRecord { Id = NextId(), PatientName = "小明", MedicationName = "阿莫西林", Dosage = "125mg", Frequency = "每日三次", Doctor = "陈静", Date = DateTime.Now });
    }

    public int GenerateId() => NextId();

    // 统计方法
    public int TotalBeds => Beds.Count;
    public int AvailableBeds => Beds.Count(b => b.Status == "空闲");
    public int OccupiedBeds => Beds.Count(b => b.Status == "占用");
    public int MaintenanceBeds => Beds.Count(b => b.Status == "维修");
    public int TodayAdmissions => Admissions.Count(a => a.AdmissionDate.Date == DateTime.Today);
    public int TodayDischarges => Discharges.Count(d => d.DischargeDate.Date == DateTime.Today);
    public int InPatientCount => Patients.Count(p => p.Status == "在院");
}
