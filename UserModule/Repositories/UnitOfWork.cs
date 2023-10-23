using Microsoft.EntityFrameworkCore.Storage;
using UserModule.Data;

namespace UserModule.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(UserDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
        Users ??= new UserRepository(_context);
    }

    public IUserRepository Users { get; }

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
        _logger.LogInformation("Transaction started.");
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
            _logger.LogInformation("Transaction committed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction commit. Rolling back...");
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
            _logger.LogInformation("Changes saved to the database.");
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
        _logger.LogWarning("Transaction rolled back.");
    }

    public void Dispose()
    {
        _context.Dispose();
        _logger.LogInformation("UnitOfWork disposed, database context released.");
    }
}