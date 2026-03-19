using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Models;

namespace MyFirstApp.Data;

public class HospitalDbContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Ward> Wards => Set<Ward>();
    public DbSet<Bed> Beds => Set<Bed>();
    public DbSet<AdmissionRecord> Admissions => Set<AdmissionRecord>();
    public DbSet<DischargeRecord> Discharges => Set<DischargeRecord>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<MedicationRecord> MedicationRecords => Set<MedicationRecord>();

    private static string DbPath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = Path.Combine(folder, "HospitalWard");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "hospital.db");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Patient
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(50);
            e.Property(x => x.Gender).HasMaxLength(10);
            e.Property(x => x.Phone).HasMaxLength(20);
            e.Property(x => x.IdCard).HasMaxLength(20);
            e.Property(x => x.BedNumber).HasMaxLength(20);
            e.Property(x => x.DepartmentName).HasMaxLength(50);
            e.Property(x => x.Status).HasMaxLength(20);
        });

        // Doctor
        modelBuilder.Entity<Doctor>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(50);
            e.Property(x => x.Gender).HasMaxLength(10);
            e.Property(x => x.Department).HasMaxLength(50);
            e.Property(x => x.Title).HasMaxLength(50);
            e.Property(x => x.Phone).HasMaxLength(20);
        });

        // Nurse
        modelBuilder.Entity<Nurse>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(50);
            e.Property(x => x.Gender).HasMaxLength(10);
            e.Property(x => x.Department).HasMaxLength(50);
            e.Property(x => x.Phone).HasMaxLength(20);
        });

        // Department
        modelBuilder.Entity<Department>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(50);
            e.Property(x => x.Description).HasMaxLength(200);
            e.Property(x => x.Director).HasMaxLength(50);
        });

        // Ward
        modelBuilder.Entity<Ward>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.WardNumber).HasMaxLength(20);
            e.Property(x => x.Type).HasMaxLength(20);
            e.Property(x => x.Department).HasMaxLength(50);
        });

        // Bed
        modelBuilder.Entity<Bed>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.BedNumber).HasMaxLength(20);
            e.Property(x => x.WardNumber).HasMaxLength(20);
            e.Property(x => x.Status).HasMaxLength(20);
            e.Property(x => x.PatientName).HasMaxLength(50);
        });

        // AdmissionRecord
        modelBuilder.Entity<AdmissionRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PatientName).HasMaxLength(50);
            e.Property(x => x.BedNumber).HasMaxLength(20);
            e.Property(x => x.Department).HasMaxLength(50);
            e.Property(x => x.Doctor).HasMaxLength(50);
            e.Property(x => x.Diagnosis).HasMaxLength(200);
        });

        // DischargeRecord
        modelBuilder.Entity<DischargeRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PatientName).HasMaxLength(50);
            e.Property(x => x.BedNumber).HasMaxLength(20);
            e.Property(x => x.Department).HasMaxLength(50);
            e.Property(x => x.TotalCost).HasColumnType("decimal(18,2)");
        });

        // MedicalRecord
        modelBuilder.Entity<MedicalRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PatientName).HasMaxLength(50);
            e.Property(x => x.Doctor).HasMaxLength(50);
            e.Property(x => x.Diagnosis).HasMaxLength(200);
            e.Property(x => x.Treatment).HasMaxLength(200);
        });

        // MedicationRecord
        modelBuilder.Entity<MedicationRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PatientName).HasMaxLength(50);
            e.Property(x => x.MedicationName).HasMaxLength(100);
            e.Property(x => x.Dosage).HasMaxLength(50);
            e.Property(x => x.Frequency).HasMaxLength(50);
            e.Property(x => x.Doctor).HasMaxLength(50);
        });

        // ===== 种子数据 =====
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // 科室
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "内科", Description = "负责内科疾病诊疗", Director = "王主任" },
            new Department { Id = 2, Name = "外科", Description = "负责外科手术及术后恢复", Director = "李主任" },
            new Department { Id = 3, Name = "儿科", Description = "负责儿童疾病诊疗", Director = "赵主任" },
            new Department { Id = 4, Name = "妇产科", Description = "负责妇产科疾病诊疗", Director = "孙主任" },
            new Department { Id = 5, Name = "急诊科", Description = "负责急诊抢救", Director = "周主任" }
        );

        // 医生
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 6, Name = "张伟", Gender = "男", Department = "内科", Title = "主任医师", Phone = "13800000001" },
            new Doctor { Id = 7, Name = "刘洋", Gender = "女", Department = "外科", Title = "副主任医师", Phone = "13800000002" },
            new Doctor { Id = 8, Name = "陈静", Gender = "女", Department = "儿科", Title = "主治医师", Phone = "13800000003" },
            new Doctor { Id = 9, Name = "王强", Gender = "男", Department = "妇产科", Title = "主治医师", Phone = "13800000004" },
            new Doctor { Id = 10, Name = "赵敏", Gender = "女", Department = "急诊科", Title = "住院医师", Phone = "13800000005" }
        );

        // 护士
        modelBuilder.Entity<Nurse>().HasData(
            new Nurse { Id = 11, Name = "林小红", Gender = "女", Department = "内科", Phone = "13900000001" },
            new Nurse { Id = 12, Name = "黄丽", Gender = "女", Department = "外科", Phone = "13900000002" },
            new Nurse { Id = 13, Name = "吴芳", Gender = "女", Department = "儿科", Phone = "13900000003" },
            new Nurse { Id = 14, Name = "郑婷", Gender = "女", Department = "急诊科", Phone = "13900000004" }
        );

        // 病房
        modelBuilder.Entity<Ward>().HasData(
            new Ward { Id = 15, WardNumber = "A区-101", Type = "普通", BedCount = 4, Department = "内科" },
            new Ward { Id = 16, WardNumber = "A区-102", Type = "普通", BedCount = 4, Department = "内科" },
            new Ward { Id = 17, WardNumber = "B区-201", Type = "VIP", BedCount = 2, Department = "外科" },
            new Ward { Id = 18, WardNumber = "B区-202", Type = "普通", BedCount = 4, Department = "外科" },
            new Ward { Id = 19, WardNumber = "C区-301", Type = "ICU", BedCount = 2, Department = "急诊科" },
            new Ward { Id = 20, WardNumber = "D区-401", Type = "普通", BedCount = 3, Department = "儿科" }
        );

        // 床位
        modelBuilder.Entity<Bed>().HasData(
            new Bed { Id = 21, BedNumber = "A101-01", WardNumber = "A区-101", Status = "占用", PatientName = "张三" },
            new Bed { Id = 22, BedNumber = "A101-02", WardNumber = "A区-101", Status = "空闲", PatientName = "" },
            new Bed { Id = 23, BedNumber = "A101-03", WardNumber = "A区-101", Status = "占用", PatientName = "王五" },
            new Bed { Id = 24, BedNumber = "A101-04", WardNumber = "A区-101", Status = "维修", PatientName = "" },
            new Bed { Id = 25, BedNumber = "A102-01", WardNumber = "A区-102", Status = "空闲", PatientName = "" },
            new Bed { Id = 26, BedNumber = "A102-02", WardNumber = "A区-102", Status = "占用", PatientName = "赵六" },
            new Bed { Id = 27, BedNumber = "B201-01", WardNumber = "B区-201", Status = "占用", PatientName = "李四" },
            new Bed { Id = 28, BedNumber = "B201-02", WardNumber = "B区-201", Status = "空闲", PatientName = "" },
            new Bed { Id = 29, BedNumber = "B202-01", WardNumber = "B区-202", Status = "空闲", PatientName = "" },
            new Bed { Id = 30, BedNumber = "B202-02", WardNumber = "B区-202", Status = "空闲", PatientName = "" },
            new Bed { Id = 31, BedNumber = "C301-01", WardNumber = "C区-301", Status = "占用", PatientName = "孙七" },
            new Bed { Id = 32, BedNumber = "C301-02", WardNumber = "C区-301", Status = "空闲", PatientName = "" },
            new Bed { Id = 33, BedNumber = "D401-01", WardNumber = "D区-401", Status = "空闲", PatientName = "" },
            new Bed { Id = 34, BedNumber = "D401-02", WardNumber = "D区-401", Status = "占用", PatientName = "小明" },
            new Bed { Id = 35, BedNumber = "D401-03", WardNumber = "D区-401", Status = "空闲", PatientName = "" }
        );

        // 患者
        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 36, Name = "张三", Gender = "男", Age = 45, Phone = "13700000001", IdCard = "110101198001010011", AdmissionDate = new DateTime(2026, 3, 13), BedNumber = "A101-01", DepartmentName = "内科", Status = "在院" },
            new Patient { Id = 37, Name = "李四", Gender = "男", Age = 60, Phone = "13700000002", IdCard = "110101196501020022", AdmissionDate = new DateTime(2026, 3, 15), BedNumber = "B201-01", DepartmentName = "外科", Status = "在院" },
            new Patient { Id = 38, Name = "王五", Gender = "女", Age = 32, Phone = "13700000003", IdCard = "110101199301030033", AdmissionDate = new DateTime(2026, 3, 17), BedNumber = "A101-03", DepartmentName = "内科", Status = "在院" },
            new Patient { Id = 39, Name = "赵六", Gender = "男", Age = 28, Phone = "13700000004", IdCard = "110101199701040044", AdmissionDate = new DateTime(2026, 3, 16), BedNumber = "A102-02", DepartmentName = "内科", Status = "在院" },
            new Patient { Id = 40, Name = "孙七", Gender = "女", Age = 55, Phone = "13700000005", IdCard = "110101197001050055", AdmissionDate = new DateTime(2026, 3, 11), BedNumber = "C301-01", DepartmentName = "急诊科", Status = "在院" },
            new Patient { Id = 41, Name = "小明", Gender = "男", Age = 8, Phone = "13700000006", IdCard = "110101201801060066", AdmissionDate = new DateTime(2026, 3, 18), BedNumber = "D401-02", DepartmentName = "儿科", Status = "在院" }
        );

        // 入院记录
        modelBuilder.Entity<AdmissionRecord>().HasData(
            new AdmissionRecord { Id = 42, PatientName = "张三", BedNumber = "A101-01", Department = "内科", Doctor = "张伟", AdmissionDate = new DateTime(2026, 3, 13), Diagnosis = "待诊断" },
            new AdmissionRecord { Id = 43, PatientName = "李四", BedNumber = "B201-01", Department = "外科", Doctor = "刘洋", AdmissionDate = new DateTime(2026, 3, 15), Diagnosis = "待诊断" },
            new AdmissionRecord { Id = 44, PatientName = "王五", BedNumber = "A101-03", Department = "内科", Doctor = "张伟", AdmissionDate = new DateTime(2026, 3, 17), Diagnosis = "待诊断" },
            new AdmissionRecord { Id = 45, PatientName = "赵六", BedNumber = "A102-02", Department = "内科", Doctor = "张伟", AdmissionDate = new DateTime(2026, 3, 16), Diagnosis = "待诊断" },
            new AdmissionRecord { Id = 46, PatientName = "孙七", BedNumber = "C301-01", Department = "急诊科", Doctor = "赵敏", AdmissionDate = new DateTime(2026, 3, 11), Diagnosis = "待诊断" },
            new AdmissionRecord { Id = 47, PatientName = "小明", BedNumber = "D401-02", Department = "儿科", Doctor = "陈静", AdmissionDate = new DateTime(2026, 3, 18), Diagnosis = "待诊断" }
        );

        // 出院记录
        modelBuilder.Entity<DischargeRecord>().HasData(
            new DischargeRecord { Id = 48, PatientName = "周八", BedNumber = "A102-01", Department = "内科", DischargeDate = new DateTime(2026, 3, 17), TotalCost = 5680.50m },
            new DischargeRecord { Id = 49, PatientName = "吴九", BedNumber = "B202-01", Department = "外科", DischargeDate = new DateTime(2026, 3, 16), TotalCost = 12350.00m }
        );

        // 诊疗记录
        modelBuilder.Entity<MedicalRecord>().HasData(
            new MedicalRecord { Id = 50, PatientName = "张三", Doctor = "张伟", Date = new DateTime(2026, 3, 14), Diagnosis = "高血压", Treatment = "降压药物治疗" },
            new MedicalRecord { Id = 51, PatientName = "李四", Doctor = "刘洋", Date = new DateTime(2026, 3, 16), Diagnosis = "骨折", Treatment = "手术复位固定" },
            new MedicalRecord { Id = 52, PatientName = "孙七", Doctor = "赵敏", Date = new DateTime(2026, 3, 12), Diagnosis = "急性胃炎", Treatment = "输液+药物治疗" },
            new MedicalRecord { Id = 53, PatientName = "小明", Doctor = "陈静", Date = new DateTime(2026, 3, 18), Diagnosis = "支气管炎", Treatment = "雾化+抗生素治疗" }
        );

        // 用药记录
        modelBuilder.Entity<MedicationRecord>().HasData(
            new MedicationRecord { Id = 54, PatientName = "张三", MedicationName = "硝苯地平", Dosage = "10mg", Frequency = "每日两次", Doctor = "张伟", Date = new DateTime(2026, 3, 14) },
            new MedicationRecord { Id = 55, PatientName = "李四", MedicationName = "布洛芬", Dosage = "200mg", Frequency = "每日三次", Doctor = "刘洋", Date = new DateTime(2026, 3, 16) },
            new MedicationRecord { Id = 56, PatientName = "孙七", MedicationName = "奥美拉唑", Dosage = "20mg", Frequency = "每日一次", Doctor = "赵敏", Date = new DateTime(2026, 3, 12) },
            new MedicationRecord { Id = 57, PatientName = "小明", MedicationName = "阿莫西林", Dosage = "125mg", Frequency = "每日三次", Doctor = "陈静", Date = new DateTime(2026, 3, 18) }
        );
    }
}
