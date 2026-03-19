using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyFirstApp.Data;

/// <summary>
/// 通用 EF Core 仓储实现。
/// 所有模型共享同一套 CRUD 逻辑，新增模型无需编写新的 Repository 类。
/// </summary>
public class DbRepository<T> : IRepository<T> where T : class
{
    private readonly HospitalDbContext _context;
    private readonly DbSet<T> _dbSet;

    public ObservableCollection<T> Items { get; private set; } = new();

    public DbRepository(HospitalDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public void Load()
    {
        var data = _dbSet.ToList();
        Items = new ObservableCollection<T>(data);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        Items.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
        Items.Remove(entity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
