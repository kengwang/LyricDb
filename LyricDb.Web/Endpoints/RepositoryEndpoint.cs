using LyricDb.Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LyricDb.Web.Endpoints;

public class RepositoryEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
    }

    public static void ConfigureApp(WebApplication app)
    {
        // ignore
    }
}

public class DbContextRepository<TEntity>(PostgresDbContext dbContext) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _entities = dbContext.Set<TEntity>();


    public async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var ret = await _entities.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ret.Entity;
    }

    public async Task<TEntity?> ReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entities.FindAsync([id], cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        AttachIfNot(entity);
        var entry = dbContext.Entry(entity);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await ReadAsync(id, cancellationToken);
        if (entity == null)
        {
            return;
        }

        _entities.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.AsQueryable());
    }

    private void AttachIfNot(TEntity entity)
    {
        var entry = dbContext.ChangeTracker.Entries()
            .FirstOrDefault(ent => ent.Entity == entity);

        if (entry != null)
        {
            return;
        }

        _entities.Attach(entity);
    }
}