using System.Collections.ObjectModel;
using System.Linq;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public class MyFavoritesViewModel : ViewModelBase
{
    public ObservableCollection<ForumFavorite> Items { get; }

    public MyFavoritesViewModel()
    {
        var ds = DataService.Instance;
        Items = new ObservableCollection<ForumFavorite>(ds.ForumFavorites.Where(x => x.Username == "张三").OrderByDescending(x => x.FavoritedAt));
    }
}
