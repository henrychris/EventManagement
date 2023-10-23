using EventModule.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventModule.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(EventDbContext context)
    {
        _context = context;
        Events ??= new EventRepository(_context);
    }

    public IEventRepository Events { get; }

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        if (_transaction == null)
        {
            return;
        }

        try
        {
            _context.SaveChanges();
            _transaction.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CompleteAsync()
    {
        if (_transaction != null)
        {
            Commit();
        }
        else
        {
            await _context.SaveChangesAsync();
        }
    }

    public void Rollback()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}