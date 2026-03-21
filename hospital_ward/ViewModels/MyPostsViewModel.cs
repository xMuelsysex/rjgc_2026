using System.Collections.ObjectModel;
using System.Linq;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public class MyPostsViewModel : ViewModelBase
{
    public ObservableCollection<ForumPost> Items { get; }

    public MyPostsViewModel()
    {
        var ds = DataService.Instance;
        Items = new ObservableCollection<ForumPost>(ds.ForumPosts.Where(x => x.Author == "张三").OrderByDescending(x => x.CreatedAt));
    }
}
