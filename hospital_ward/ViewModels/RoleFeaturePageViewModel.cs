using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyFirstApp.ViewModels;

public partial class RoleFeaturePageViewModel : ViewModelBase
{
    public string Title { get; }

    public string Summary { get; }

    public string Audience { get; }

    public ObservableCollection<string> CapabilityItems { get; }

    public RoleFeaturePageViewModel(string title, string summary, string audience, IEnumerable<string> capabilities)
    {
        Title = title;
        Summary = summary;
        Audience = audience;
        CapabilityItems = new ObservableCollection<string>(capabilities);
    }
}
