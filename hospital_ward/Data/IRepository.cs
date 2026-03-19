using System.Collections.ObjectModel;

namespace MyFirstApp.Data;

/// <summary>
/// 泛型仓储接口，定义统一的 CRUD 操作。
/// 新增模型只需使用 DbRepository&lt;T&gt; 即可，无需编写新的仓储类。
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>从数据库加载的数据集合（可绑定到 UI）</summary>
    ObservableCollection<T> Items { get; }

    /// <summary>从数据库重新加载所有数据到 Items</summary>
    void Load();

    /// <summary>添加实体并同步到 Items</summary>
    void Add(T entity);

    /// <summary>标记实体为已修改</summary>
    void Update(T entity);

    /// <summary>删除实体并从 Items 中移除</summary>
    void Remove(T entity);

    /// <summary>将所有更改持久化到数据库</summary>
    void SaveChanges();
}
