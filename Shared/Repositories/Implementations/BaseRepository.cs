using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories.Interfaces;

namespace Shared.Repositories.Implementations;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    protected BaseRepository(DbContext context)
    {
        Context = context;
    }

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task RemoveByIdAsync(string id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity is not null)
        {
            Context.Remove(entity);
        }
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<bool> Exists(string id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        return entity is null;
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Entry(entity).State = EntityState.Modified;
    }
}