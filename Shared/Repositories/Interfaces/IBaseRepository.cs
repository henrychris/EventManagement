using System.Linq.Expressions;

namespace Shared.Repositories.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task RemoveByIdAsync(string id);
    void Remove(TEntity entity);
    void RemoveRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> Exists(string id);
    void Update(TEntity entity);
}