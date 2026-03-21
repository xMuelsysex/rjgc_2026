using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstApp.Models;

public partial class ForumFavorite : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _postTitle = string.Empty;
    [ObservableProperty] private string _category = string.Empty;
    [ObservableProperty] private DateTime _favoritedAt = DateTime.Now;
}
