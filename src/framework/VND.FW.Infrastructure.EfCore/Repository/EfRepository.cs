using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.FW.Infrastructure.EfCore.Repository
{
  public class EfRepositoryAsync<TEntity>
        : EfRepositoryAsync<DbContext, TEntity>, IEfRepositoryAsync<TEntity>
        where TEntity : class, IEntity
  {
    public EfRepositoryAsync(DbContext dbContext) : base(dbContext)
    {
    }
  }

  public class EfQueryRepository<TEntity>
      : EfQueryRepository<DbContext, TEntity>, IEfQueryRepository<TEntity>
      where TEntity : class, IEntity
  {
    public EfQueryRepository(DbContext dbContext) : base(dbContext)
    {
    }
  }

  public class EfRepositoryAsync<TDbContext, TEntity> : IEfRepositoryAsync<TDbContext, TEntity>
      where TDbContext : DbContext
      where TEntity : class, IEntity
  {
    private readonly TDbContext _dbContext;

    public EfRepositoryAsync(TDbContext dbContext)
    {
      _dbContext = dbContext;
      _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
      Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntityEntry = _dbContext.Entry(entity);
      await _dbContext.Set<TEntity>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();
      return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
      _dbContext.Entry(entity).State = EntityState.Deleted;
      await _dbContext.SaveChangesAsync();
      return await Task.FromResult(entity);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
      _dbContext.Entry(entity).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
      return await Task.FromResult(entity);
    }
  }

  public class EfQueryRepository<TDbContext, TEntity> : IEfQueryRepository<TDbContext, TEntity>
      where TDbContext : DbContext
      where TEntity : class, IEntity
  {
    private readonly TDbContext _dbContext;

    public EfQueryRepository(TDbContext dbContext)
    {
      _dbContext = dbContext;
      _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public IQueryable<TEntity> Queryable() => _dbContext.Set<TEntity>();
  }
}
