using Microsoft.EntityFrameworkCore.Storage;
using UserModule.Data;

namespace UserModule.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(UserDbContext context)
    {
        _context = context;
        Users ??= new UserRepository(_context);
    }

    public IUserRepository Users { get; }

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