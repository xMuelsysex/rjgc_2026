# 医院病房病床管理系统 - Avalonia 全栈实现

将 CSDN 博客中描述的 Spring Boot 医院病房病床管理系统功能，全部使用 **Avalonia UI + CommunityToolkit.Mvvm** 在桌面端实现。使用内存数据（`ObservableCollection`）模拟数据库，保持项目纯 Avalonia 无额外依赖。

## Proposed Changes

### Models 组件

创建 10 个数据模型类到 `Models/` 目录：

#### [NEW] [Patient.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Patient.cs)
患者模型：Id, Name, Gender, Age, Phone, IdCard, AdmissionDate, BedNumber, DepartmentName, Status

#### [NEW] [Doctor.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Doctor.cs)
医生模型：Id, Name, Gender, Department, Title, Phone

#### [NEW] [Nurse.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Nurse.cs)
护士模型：Id, Name, Gender, Department, Phone

#### [NEW] [Department.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Department.cs)
科室模型：Id, Name, Description, Director

#### [NEW] [Ward.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Ward.cs)
病房模型：Id, WardNumber, Type(普通/VIP/ICU), BedCount, Department

#### [NEW] [Bed.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/Bed.cs)
床位模型：Id, BedNumber, WardNumber, Status(空闲/占用/维修), PatientName

#### [NEW] [AdmissionRecord.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/AdmissionRecord.cs)
入院登记：Id, PatientName, BedNumber, Department, Doctor, AdmissionDate, Diagnosis

#### [NEW] [DischargeRecord.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/DischargeRecord.cs)
出院登记：Id, PatientName, BedNumber, Department, DischargeDate, TotalCost

#### [NEW] [MedicalRecord.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/MedicalRecord.cs)
诊疗信息：Id, PatientName, Doctor, Date, Diagnosis, Treatment

#### [NEW] [MedicationRecord.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Models/MedicationRecord.cs)
用药信息：Id, PatientName, MedicationName, Dosage, Frequency, Doctor, Date

---

### Services 组件

#### [NEW] [DataService.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Services/DataService.cs)
单例数据服务，用 `ObservableCollection` 管理所有模拟数据，提供各模块的增删改查方法。所有 ViewModel 统一从此服务读取数据。

---

### ViewModels 组件

#### [MODIFY] [DashboardViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/DashboardViewModel.cs)
从 DataService 计算实时统计数据（总床位、空闲床位、今日入院/出院等）。

#### [MODIFY] [BedManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/BedManagementViewModel.cs)
改用 [Bed](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/BedManagementViewModel.cs#18-19) 模型，支持搜索、添加、编辑、删除床位。删除旧的 [BedItem](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/BedManagementViewModel.cs#18-19) record。

#### [MODIFY] [PatientManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/PatientManagementViewModel.cs)
实现完整患者管理：列表展示、搜索、添加、编辑、删除。

#### [NEW] [DoctorManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/DoctorManagementViewModel.cs)
医生管理 CRUD + 搜索。

#### [NEW] [NurseManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/NurseManagementViewModel.cs)
护士管理 CRUD + 搜索。

#### [NEW] [DepartmentManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/DepartmentManagementViewModel.cs)
科室管理 CRUD + 搜索。

#### [NEW] [WardManagementViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/WardManagementViewModel.cs)
病房管理 CRUD + 搜索。

#### [NEW] [AdmissionViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/AdmissionViewModel.cs)
入院登记 CRUD + 搜索。

#### [NEW] [DischargeViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/DischargeViewModel.cs)
出院登记 CRUD + 搜索。

#### [NEW] [MedicalRecordViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/MedicalRecordViewModel.cs)
诊疗信息 CRUD + 搜索。

#### [NEW] [MedicationViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/MedicationViewModel.cs)
用药信息 CRUD + 搜索。

#### [MODIFY] [MainWindowViewModel.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/ViewModels/MainWindowViewModel.cs)
新增所有导航项，更新 NavigateTo 方法映射所有新 ViewModel。

---

### Views 组件

每个新 ViewModel 对应一个 View（[.axaml](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml) + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)），统一布局：顶部标题 + 搜索栏/操作按钮 + DataGrid 列表。

#### [MODIFY] [DashboardView.axaml](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Views/DashboardView.axaml)
增加床位状态概览和占用率百分比。

#### [MODIFY] [BedManagementView.axaml](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Views/BedManagementView.axaml)
完善 DataGrid 列（增加病房号列），搜索框绑定。

#### [MODIFY] [PatientManagementView.axaml](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Views/PatientManagementView.axaml)
完整 DataGrid 表格 + 搜索 + 操作按钮。

#### [NEW] `DoctorManagementView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `NurseManagementView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `DepartmentManagementView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `WardManagementView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `AdmissionView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `DischargeView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `MedicalRecordView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)
#### [NEW] `MedicationView.axaml` + [.axaml.cs](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/App.axaml.cs)

---

### 主窗体

#### [MODIFY] [MainWindow.axaml](file:///d:/github/ruanjiangngcheng_2026/MyFirstApp/Views/MainWindow.axaml)
在 `Window.Resources` 中添加新模块的图标 PathData，侧边栏扩展显示所有导航项。

---

## Verification Plan

### Automated Tests
```bash
cd d:\github\ruanjiangngcheng_2026\MyFirstApp
dotnet build
```
编译通过即代表所有 View ↔ ViewModel 的绑定和类型匹配正确。

### Manual Verification
1. 运行 `dotnet run`，程序窗口正常弹出
2. 点击侧边栏每个导航项，确认页面正确切换
3. 在各管理页面测试：搜索、添加、删除功能是否正常响应
