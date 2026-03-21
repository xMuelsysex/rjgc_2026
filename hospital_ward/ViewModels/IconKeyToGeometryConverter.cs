using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MyFirstApp.ViewModels;

public class IconKeyToGeometryConverter : IValueConverter
{
    public Geometry? HomeIcon { get; set; }
    public Geometry? BedIcon { get; set; }
    public Geometry? PersonIcon { get; set; }
    public Geometry? DoctorIcon { get; set; }
    public Geometry? NurseIcon { get; set; }
    public Geometry? DepartmentIcon { get; set; }
    public Geometry? WardIcon { get; set; }
    public Geometry? AdmissionIcon { get; set; }
    public Geometry? DischargeIcon { get; set; }
    public Geometry? MedicalIcon { get; set; }
    public Geometry? MedicationIcon { get; set; }
    public Geometry? MoneyIcon { get; set; }
    public Geometry? ApprovalIcon { get; set; }
    public Geometry? OrderIcon { get; set; }
    public Geometry? CareIcon { get; set; }
    public Geometry? PharmacyIcon { get; set; }
    public Geometry? InventoryIcon { get; set; }
    public Geometry? DeviceIcon { get; set; }
    public Geometry? ForumIcon { get; set; }
    public Geometry? SettingsIcon { get; set; }
    public Geometry? ProfileIcon { get; set; }
    public Geometry? LockIcon { get; set; }
    public Geometry? PublishIcon { get; set; }
    public Geometry? FavoriteIcon { get; set; }
    public Geometry? DefaultIcon { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "Home" => HomeIcon,
            "Bed" => BedIcon,
            "Person" => PersonIcon,
            "Doctor" => DoctorIcon,
            "Nurse" => NurseIcon,
            "Department" => DepartmentIcon,
            "Ward" => WardIcon,
            "Admission" => AdmissionIcon,
            "Discharge" => DischargeIcon,
            "Medical" => MedicalIcon,
            "Medication" => MedicationIcon,
            "Money" => MoneyIcon,
            "Approval" => ApprovalIcon,
            "Order" => OrderIcon,
            "Care" => CareIcon,
            "Pharmacy" => PharmacyIcon,
            "Inventory" => InventoryIcon,
            "Device" => DeviceIcon,
            "Forum" => ForumIcon,
            "Settings" => SettingsIcon,
            "Profile" => ProfileIcon,
            "Lock" => LockIcon,
            "Publish" => PublishIcon,
            "Favorite" => FavoriteIcon,
            _ => DefaultIcon
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
