using System.Linq.Expressions;

namespace EcommerceMicroserviceCase.Stock.Api.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync<T>(
        T id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<TEntity>> GetByQueryAsync(
        Expression<Func<TEntity, bool>> predicate, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, 
        CancellationToken cancellationToken = default);
    
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    
    Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);
    
    Task UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync<T>(
        T id,
        CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}