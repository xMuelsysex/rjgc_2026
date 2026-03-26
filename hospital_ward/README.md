# 智能病房管理系统

基于 [`Avalonia UI`](https://avaloniaui.net/) 与 [`.NET 8`](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0) 构建的桌面端医院病房管理系统示例项目。

当前项目已经从最初的演示型骨架，逐步扩展为包含多角色登录、住院流程管理、费用结算、药品与设备管理、论坛互动、系统管理等模块的完整原型系统。

---

## 1. 项目概述

本系统围绕医院住院业务的核心场景展开，支持四类角色：

- 管理员
- 患者
- 医生
- 护士

系统采用本地 SQLite 数据库存储，并通过 [`Entity Framework Core`](hospital_ward/Data/HospitalDbContext.cs:8) 进行数据访问，使用 [`MVVM`](hospital_ward/ViewModels/ViewModelBase.cs:1) 模式组织 Avalonia 桌面界面。

---

## 2. 技术栈

- 桌面 UI：[`Avalonia`](hospital_ward/MyFirstApp.csproj:15)
- MVVM：[`CommunityToolkit.Mvvm`](hospital_ward/MyFirstApp.csproj:25)
- 数据访问：[`Entity Framework Core SQLite`](hospital_ward/MyFirstApp.csproj:30)
- 运行平台：Windows / .NET 8

---

## 3. 当前已实现模块

### 3.1 通用能力

- 多角色登录入口
- 注册页面
- 当前用户上下文
- 角色化动态菜单
- SQLite 本地持久化
- 基础 CRUD
- 搜索过滤
- 角色数据隔离
- 业务联动

### 3.2 管理员端

- [`系统首页`](hospital_ward/ViewModels/DashboardViewModel.cs:1)
- [`患者管理`](hospital_ward/ViewModels/PatientManagementViewModel.cs:11)
- [`医生管理`](hospital_ward/ViewModels/DoctorManagementViewModel.cs:1)
- [`护士管理`](hospital_ward/ViewModels/NurseManagementViewModel.cs:1)
- [`科室管理`](hospital_ward/ViewModels/DepartmentManagementViewModel.cs:1)
- [`病房管理`](hospital_ward/ViewModels/WardManagementViewModel.cs:1)
- [`病房床位`](hospital_ward/ViewModels/BedManagementViewModel.cs:1)
- [`入院登记`](hospital_ward/ViewModels/AdmissionViewModel.cs:1)
- [`出院登记`](hospital_ward/ViewModels/DischargeViewModel.cs:11)
- [`费用管理`](hospital_ward/ViewModels/BillingManagementViewModel.cs:11)
- [`出院申请`](hospital_ward/ViewModels/DischargeRequestViewModel.cs:11)
- [`诊疗信息`](hospital_ward/ViewModels/MedicalRecordViewModel.cs:11)
- [`用药信息`](hospital_ward/ViewModels/MedicationViewModel.cs:11)
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:11)
- [`护理管理`](hospital_ward/ViewModels/NursingManagementViewModel.cs:11)
- [`药品信息`](hospital_ward/ViewModels/DrugCatalogViewModel.cs:11)
- [`药品入库`](hospital_ward/ViewModels/DrugInventoryViewModel.cs:11)
- [`仪器信息`](hospital_ward/ViewModels/MedicalDeviceViewModel.cs:11)
- [`患者论坛`](hospital_ward/ViewModels/ForumViewModel.cs:11)
- [`系统管理`](hospital_ward/ViewModels/SystemManagementViewModel.cs:11)
- [`用户资料`](hospital_ward/ViewModels/UserProfileViewModel.cs:1)

### 3.3 患者端

- [`个人中心`](hospital_ward/ViewModels/PatientCenterViewModel.cs:1)
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:11)
- [`修改密码`](hospital_ward/ViewModels/ChangePasswordViewModel.cs:1)
- [`入院登记`](hospital_ward/ViewModels/AdmissionViewModel.cs:1)
- [`出院登记`](hospital_ward/ViewModels/DischargeViewModel.cs:11)
- [`费用管理`](hospital_ward/ViewModels/BillingManagementViewModel.cs:11)
- [`出院申请`](hospital_ward/ViewModels/DischargeRequestViewModel.cs:11)
- [`诊疗信息`](hospital_ward/ViewModels/MedicalRecordViewModel.cs:11)
- [`用药信息`](hospital_ward/ViewModels/MedicationViewModel.cs:11)
- [`我的发布`](hospital_ward/ViewModels/MyPostsViewModel.cs:1)
- [`我的收藏`](hospital_ward/ViewModels/MyFavoritesViewModel.cs:1)

### 3.4 医生端

- [`系统首页`](hospital_ward/ViewModels/DashboardViewModel.cs:1)
- [`入院登记`](hospital_ward/ViewModels/AdmissionViewModel.cs:1)
- [`出院申请`](hospital_ward/ViewModels/DischargeRequestViewModel.cs:11)
- [`诊疗信息`](hospital_ward/ViewModels/MedicalRecordViewModel.cs:11)
- [`用药信息`](hospital_ward/ViewModels/MedicationViewModel.cs:11)
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:11)
- [`护理管理`](hospital_ward/ViewModels/NursingManagementViewModel.cs:11)
- [`药品信息`](hospital_ward/ViewModels/DrugCatalogViewModel.cs:11)
- [`用户资料`](hospital_ward/ViewModels/UserProfileViewModel.cs:1)

### 3.5 护士端

- [`系统首页`](hospital_ward/ViewModels/DashboardViewModel.cs:1)
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:11)
- [`护理管理`](hospital_ward/ViewModels/NursingManagementViewModel.cs:11)
- [`用户资料`](hospital_ward/ViewModels/UserProfileViewModel.cs:1)

---

## 4. 已实现的关键增强

### 4.1 登录与注册

- 登录页：[`LoginView`](hospital_ward/Views/LoginView.axaml)
- 注册页：[`RegisterView`](hospital_ward/Views/RegisterView.axaml)
- 会话服务：[`UserSessionService`](hospital_ward/Services/UserSessionService.cs:1)

注册规则：

- 患者注册：账号、密码、确认密码、姓名、手机号、身份证号
- 医生注册：账号、密码、确认密码、姓名、手机号、科室
- 护士注册：账号、密码、确认密码、姓名、手机号、科室
- 管理员不开放注册

默认管理员账号：

- 账号：`admin`
- 密码：`admin`

### 4.2 业务联动

- [`出院申请`](hospital_ward/ViewModels/DischargeRequestViewModel.cs:71) 批准后联动 [`费用管理`](hospital_ward/ViewModels/BillingManagementViewModel.cs:11)
- [`药品入库`](hospital_ward/ViewModels/DrugInventoryViewModel.cs:71) 保存后联动 [`药品信息`](hospital_ward/ViewModels/DrugCatalogViewModel.cs:11) 库存
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:71) 新增时联动生成 [`护理管理`](hospital_ward/ViewModels/NursingManagementViewModel.cs:11) 任务

### 4.3 角色数据隔离

以下页面已实现按当前登录用户过滤：

- [`费用管理`](hospital_ward/ViewModels/BillingManagementViewModel.cs:31)
- [`出院申请`](hospital_ward/ViewModels/DischargeRequestViewModel.cs:31)
- [`患者医嘱`](hospital_ward/ViewModels/PatientOrderViewModel.cs:31)
- [`诊疗信息`](hospital_ward/ViewModels/MedicalRecordViewModel.cs:1)
- [`用药信息`](hospital_ward/ViewModels/MedicationViewModel.cs:11)
- [`护理管理`](hospital_ward/ViewModels/NursingManagementViewModel.cs:11)

---

## 5. 项目结构

```text
hospital_ward/
├─ Data/                 数据访问层与 DbContext
├─ Models/               实体模型
├─ Services/             数据服务、会话服务
├─ ViewModels/           MVVM 视图模型
├─ Views/                Avalonia 视图
├─ App.axaml             应用样式入口
├─ App.axaml.cs          应用启动入口
└─ MyFirstApp.csproj     项目文件
```

---

## 6. 环境要求

- [`.NET 8 SDK`](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)
- Windows 11 或可运行 Avalonia 桌面应用的环境
- 推荐 IDE：Visual Studio 2022 / Rider / VS Code

---

## 7. 运行方式

### 7.1 命令行运行

在项目根目录执行：

```bash
dotnet run --project .\hospital_ward\MyFirstApp.csproj
```

### 7.2 构建项目

```bash
dotnet build .\ruanjiangngcheng_2026.sln
```

### 7.3 发布项目

```bash
dotnet publish .\hospital_ward\MyFirstApp.csproj -c Release
```

---

## 8. 数据说明

- 使用 SQLite 本地数据库
- 通过 [`HospitalDbContext`](hospital_ward/Data/HospitalDbContext.cs:8) 自动创建表结构
- 项目内包含种子数据
- 当本地旧数据库与新结构不一致时，[`DataService`](hospital_ward/Services/DataService.cs:10) 会自动重建演示数据库

---

## 9. 常见问题

### 9.1 登录成功但没有跳转

当前版本已修复该问题，关键逻辑位于：

- [`LoginViewModel`](hospital_ward/ViewModels/LoginViewModel.cs:7)
- [`App.axaml.cs`](hospital_ward/App.axaml.cs:21)

### 9.2 页面空白或表格未显示

请确认已引入 [`Avalonia.Controls.DataGrid`](hospital_ward/MyFirstApp.csproj:16) 并在 [`App.axaml`](hospital_ward/App.axaml) 中正确加载样式。

### 9.3 数据结构变化后启动报错

项目已在 [`DataService`](hospital_ward/Services/DataService.cs:41) 中加入 SQLite 旧库兼容处理，通常重启即可恢复。

---

## 10. 后续建议

后续可以继续增强以下能力：

- 更严格的表单校验
- 删除确认与操作提示
- 更完整的登录用户映射
- 论坛评论与审核机制
- 首页待办与预警指标
- 更细粒度的权限控制

---

## 11. 版权与说明

本项目为教学 / 课程设计 / 原型演示用途，适合作为 Avalonia + EF Core + MVVM 的桌面系统实践参考。
