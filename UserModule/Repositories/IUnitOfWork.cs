namespace UserModule.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    void BeginTransaction();
    Task CompleteAsync();
    void Commit();
    void Rollback();
}