namespace EcommerceMicroserviceCase.StockService.Api.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync<T>(T id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync<T>(T id);
    Task SaveChangesAsync();
}