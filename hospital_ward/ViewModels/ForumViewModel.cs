using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public partial class ForumViewModel : ViewModelBase
{
    private readonly DataService _ds = DataService.Instance;
    private bool _isNewRecord;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<ForumPost> _filteredItems = new();
    [ObservableProperty] private ForumPost? _selectedItem;
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string _editTitle = string.Empty;
    [ObservableProperty] private string _editContent = string.Empty;
    [ObservableProperty] private string _editAuthor = "张三";
    [ObservableProperty] private string _editCategory = string.Empty;
    [ObservableProperty] private string _editStatus = "已发布";

    public ForumViewModel() => RefreshList();

    partial void OnSearchTextChanged(string value) => RefreshList();

    private void RefreshList()
    {
        var query = _ds.ForumPosts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var kw = SearchText.Trim();
            query = query.Where(x => x.Title.Contains(kw) || x.Author.Contains(kw) || x.Category.Contains(kw));
        }

        FilteredItems = new ObservableCollection<ForumPost>(query.OrderByDescending(x => x.CreatedAt));
    }

    [RelayCommand]
    private void Add()
    {
        _isNewRecord = true;
        EditTitle = string.Empty;
        EditContent = string.Empty;
        EditAuthor = "张三";
        EditCategory = string.Empty;
        EditStatus = "已发布";
        IsEditing = true;
    }

    [RelayCommand]
    private void Edit()
    {
        if (SelectedItem is null) return;
        _isNewRecord = false;
        EditTitle = SelectedItem.Title;
        EditContent = SelectedItem.Content;
        EditAuthor = SelectedItem.Author;
        EditCategory = SelectedItem.Category;
        EditStatus = SelectedItem.Status;
        IsEditing = true;
    }

    [RelayCommand]
    private void Save()
    {
        if (_isNewRecord)
        {
            _ds.ForumPostRepo.Add(new ForumPost
            {
                Id = _ds.GenerateId(),
                Title = EditTitle,
                Content = EditContent,
                Author = EditAuthor,
                Category = EditCategory,
                Status = EditStatus,
                CreatedAt = DateTime.Now,
                FavoriteCount = 0
            });
        }
        else if (SelectedItem is not null)
        {
            SelectedItem.Title = EditTitle;
            SelectedItem.Content = EditContent;
            SelectedItem.Author = EditAuthor;
            SelectedItem.Category = EditCategory;
            SelectedItem.Status = EditStatus;
        }

        _ds.SaveChanges();
        IsEditing = false;
        RefreshList();
    }

    [RelayCommand] private void Cancel() => IsEditing = false;

    [RelayCommand]
    private void Delete()
    {
        if (SelectedItem is null) return;
        _ds.ForumPostRepo.Remove(SelectedItem);
        _ds.SaveChanges();
        RefreshList();
    }
}
