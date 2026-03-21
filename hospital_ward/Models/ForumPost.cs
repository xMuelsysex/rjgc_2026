using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class ForumPost : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private string _author = string.Empty;
    [ObservableProperty] private string _category = string.Empty;
    [ObservableProperty] private DateTime _createdAt = DateTime.Now;
    [ObservableProperty] private int _favoriteCount;
    [ObservableProperty] private string _status = "已发布";
}
