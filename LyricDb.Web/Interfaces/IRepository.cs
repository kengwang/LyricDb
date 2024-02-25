namespace LyricDb.Web.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    public Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);
}