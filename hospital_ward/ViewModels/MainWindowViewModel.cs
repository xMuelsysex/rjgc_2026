using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Dictionary<SystemRole, List<NavigationItem>> _menuCatalog;
    private readonly UserSessionService _sessionService;

    public ObservableCollection<NavigationItem> MenuItems { get; } = new();

    [ObservableProperty]
    private ViewModelBase _currentPage = null!;

    [ObservableProperty]
    private NavigationItem _selectedMenuItem = null!;

    [ObservableProperty]
    private SystemRole _selectedRole;

    public IReadOnlyList<SystemRole> AvailableRoles { get; } = Enum.GetValues<SystemRole>();

    public string CurrentUserName => _sessionService.CurrentProfile?.DisplayName ?? "未登录";

    public string CurrentUserRoleName => _sessionService.CurrentRole.GetDisplayName();

    public string CurrentRoleDisplayName => SelectedRole.GetDisplayName();

    public string CurrentRoleDescription => SelectedRole switch
    {
        SystemRole.Admin => "管理全院基础信息、业务流程与系统运营模块。",
        SystemRole.Patient => "查看个人相关住院信息、费用、论坛与个人中心。",
        SystemRole.Doctor => "处理诊疗、用药、医嘱、护理协同与出入院流程。",
        SystemRole.Nurse => "执行患者医嘱、护理任务并维护个人资料。",
        _ => string.Empty
    };

    public MainWindowViewModel(UserSessionService sessionService)
    {
        _sessionService = sessionService;
        _menuCatalog = BuildMenuCatalog();

        SelectedRole = _sessionService.CurrentRole;
        LoadMenuForRole(SelectedRole);
    }

    partial void OnSelectedMenuItemChanged(NavigationItem value)
    {
        if (value != null)
        {
            NavigateTo(value);
        }
    }

    partial void OnSelectedRoleChanged(SystemRole value)
    {
        OnPropertyChanged(nameof(CurrentRoleDisplayName));
        OnPropertyChanged(nameof(CurrentRoleDescription));
        LoadMenuForRole(value);
    }

    private void LoadMenuForRole(SystemRole role)
    {
        MenuItems.Clear();

        foreach (var item in _menuCatalog[role])
        {
            MenuItems.Add(item);
        }

        SelectedMenuItem = MenuItems.First();
    }

    private void NavigateTo(NavigationItem item)
    {
        CurrentPage = item.ViewModelFactory();
    }

    private static Dictionary<SystemRole, List<NavigationItem>> BuildMenuCatalog()
    {
        return new Dictionary<SystemRole, List<NavigationItem>>
        {
            [SystemRole.Admin] = new List<NavigationItem>
            {
                CreateBuiltIn("系统首页", "Home", "概览全院核心运营指标。", () => new DashboardViewModel()),
                CreateBuiltIn("患者管理", "Person", "维护患者基础档案与住院状态。", () => new PatientManagementViewModel()),
                CreateBuiltIn("医生管理", "Doctor", "维护医生信息、科室归属与职称。", () => new DoctorManagementViewModel()),
                CreateBuiltIn("护士管理", "Nurse", "维护护士人员资料。", () => new NurseManagementViewModel()),
                CreateBuiltIn("科室管理", "Department", "维护医院科室与负责人配置。", () => new DepartmentManagementViewModel()),
                CreateBuiltIn("病房管理", "Ward", "维护病房分区、类型与容量。", () => new WardManagementViewModel()),
                CreateBuiltIn("病房床位", "Bed", "统一维护床位状态与分配。", () => new BedManagementViewModel()),
                CreateBuiltIn("入院登记", "Admission", "办理患者入院登记。", () => new AdmissionViewModel()),
                CreateBuiltIn("出院登记", "Discharge", "办理患者出院登记。", () => new DischargeViewModel()),
                CreateBuiltIn("费用管理", "Money", "费用、结算与账单管理。", () => new BillingManagementViewModel()),
                CreateBuiltIn("出院申请", "Approval", "审核患者出院申请流程。", () => new DischargeRequestViewModel()),
                CreateBuiltIn("诊疗信息", "Medical", "查看与维护诊疗记录。", () => new MedicalRecordViewModel()),
                CreateBuiltIn("用药信息", "Medication", "查看与维护用药记录。", () => new MedicationViewModel()),
                CreateBuiltIn("患者医嘱", "Order", "查看和管理住院患者医嘱。", () => new PatientOrderViewModel()),
                CreateBuiltIn("护理管理", "Care", "维护护理任务与执行记录。", () => new NursingManagementViewModel()),
                CreateBuiltIn("药品信息", "Pharmacy", "维护药品目录与库存基础信息。", () => new DrugCatalogViewModel()),
                CreateBuiltIn("药品入库", "Inventory", "登记药品入库与库存变动。", () => new DrugInventoryViewModel()),
                CreateBuiltIn("仪器信息", "Device", "维护医疗仪器设备台账。", () => new MedicalDeviceViewModel()),
                CreateBuiltIn("患者论坛", "Forum", "管理患者交流论坛内容。", () => new ForumViewModel()),
                CreateBuiltIn("系统管理", "Settings", "配置系统字典、角色与参数。", () => new SystemManagementViewModel()),
                CreateBuiltIn("用户资料", "Profile", "查看并维护当前用户资料。", () => new UserProfileViewModel()),
            },
            [SystemRole.Patient] = new List<NavigationItem>
            {
                CreateBuiltIn("个人中心", "Profile", "查看个人住院概况与快捷入口。", () => new PatientCenterViewModel()),
                CreateBuiltIn("患者医嘱", "Order", "查看个人医嘱执行情况。", () => new PatientOrderViewModel()),
                CreateBuiltIn("修改密码", "Lock", "维护患者账号安全设置。", () => new ChangePasswordViewModel()),
                CreateBuiltIn("入院登记", "Admission", "查看或发起本人入院登记信息。", () => new AdmissionViewModel()),
                CreateBuiltIn("出院登记", "Discharge", "查看本人出院登记记录。", () => new DischargeViewModel()),
                CreateBuiltIn("费用管理", "Money", "查看个人费用与缴费情况。", () => new BillingManagementViewModel()),
                CreateBuiltIn("出院申请", "Approval", "提交并追踪本人出院申请。", () => new DischargeRequestViewModel()),
                CreateBuiltIn("诊疗信息", "Medical", "查看本人诊疗记录。", () => new MedicalRecordViewModel()),
                CreateBuiltIn("用药信息", "Medication", "查看本人用药记录。", () => new MedicationViewModel()),
                CreateBuiltIn("我的发布", "Publish", "查看本人论坛发帖与互动。", () => new MyPostsViewModel()),
                CreateBuiltIn("我的收藏", "Favorite", "查看已收藏的论坛内容。", () => new MyFavoritesViewModel()),
            },
            [SystemRole.Doctor] = new List<NavigationItem>
            {
                CreateBuiltIn("系统首页", "Home", "查看医生工作台概览。", () => new DashboardViewModel()),
                CreateBuiltIn("入院登记", "Admission", "处理患者入院收治。", () => new AdmissionViewModel()),
                CreateBuiltIn("出院申请", "Approval", "审核或发起患者出院申请。", () => new DischargeRequestViewModel()),
                CreateBuiltIn("诊疗信息", "Medical", "维护诊疗记录。", () => new MedicalRecordViewModel()),
                CreateBuiltIn("用药信息", "Medication", "维护用药方案。", () => new MedicationViewModel()),
                CreateBuiltIn("患者医嘱", "Order", "下达并跟踪患者医嘱。", () => new PatientOrderViewModel()),
                CreateBuiltIn("护理管理", "Care", "与护理团队协作处理护理任务。", () => new NursingManagementViewModel()),
                CreateBuiltIn("药品信息", "Pharmacy", "查询药品档案与库存可用性。", () => new DrugCatalogViewModel()),
                CreateBuiltIn("用户资料", "Profile", "维护医生个人资料。", () => new UserProfileViewModel()),
            },
            [SystemRole.Nurse] = new List<NavigationItem>
            {
                CreateBuiltIn("系统首页", "Home", "查看护士工作台概览。", () => new DashboardViewModel()),
                CreateBuiltIn("患者医嘱", "Order", "执行并反馈患者医嘱。", () => new PatientOrderViewModel()),
                CreateBuiltIn("护理管理", "Care", "处理日常护理任务。", () => new NursingManagementViewModel()),
                CreateBuiltIn("用户资料", "Profile", "维护护士个人资料。", () => new UserProfileViewModel()),
            }
        };
    }

    private static NavigationItem CreateBuiltIn(string label, string iconKey, string description, Func<ViewModelBase> factory)
        => new(label, iconKey, description, factory);

    private static NavigationItem CreatePlaceholder(string label, string iconKey, string summary, string audience, string capabilities)
        => new(
            label,
            iconKey,
            summary,
            () => new RoleFeaturePageViewModel(label, summary, audience, capabilities.Split('、').ToArray()));
}

public enum SystemRole
{
    Admin,
    Patient,
    Doctor,
    Nurse
}

public static class SystemRoleExtensions
{
    public static string GetDisplayName(this SystemRole role) => role switch
    {
        SystemRole.Admin => "管理员",
        SystemRole.Patient => "患者",
        SystemRole.Doctor => "医生",
        SystemRole.Nurse => "护士",
        _ => role.ToString()
    };
}

public class SystemRoleDisplayConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, global::System.Globalization.CultureInfo culture)
        => value is SystemRole role ? role.GetDisplayName() : value?.ToString();

    public object? ConvertBack(object? value, Type targetType, object? parameter, global::System.Globalization.CultureInfo culture)
        => throw new NotSupportedException();
}

public record NavigationItem(string Label, string IconKey, string Description, Func<ViewModelBase> ViewModelFactory);
