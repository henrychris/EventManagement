namespace EventModule.Repositories;

public interface IUnitOfWork : IDisposable
{
    IEventRepository Events { get; }
    void BeginTransaction();
    Task CompleteAsync();
    void Commit();
    void Rollback();
}