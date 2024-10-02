using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KALS.Repository.Interface;

public interface IUnitOfWork: IGenericRepositoryFactory, IDisposable
{
    int Commit();
    Task<int> CommitAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task RollbackTransactionAsync(IDbContextTransaction transaction);


}
public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    TContext Context { get; }
}