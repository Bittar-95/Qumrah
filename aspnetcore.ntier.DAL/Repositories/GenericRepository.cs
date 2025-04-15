using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace aspnetcore.ntier.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
{
    private readonly AspNetCoreNTierDbContext _aspNetCoreNTierDbContext;
    public GenericRepository(AspNetCoreNTierDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _aspNetCoreNTierDbContext.AddAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
    {
        await _aspNetCoreNTierDbContext.AddRangeAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Remove(entity);
        return await _aspNetCoreNTierDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        return await _aspNetCoreNTierDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public DbSet<TEntity> Get()
    {
        return _aspNetCoreNTierDbContext.Set<TEntity>();
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var en = _aspNetCoreNTierDbContext.Set<TEntity>().AsQueryable();

        if (includes is not null)
        {
            foreach (var item in includes)
            {
                en = en.Include(item);
            }
        }
        if (filter is not null)
        {
            en = en.Where(filter);
        }

        return await en.ToListAsync();
    }

    public IPagedList<TEntity> GetPagedList(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var en = _aspNetCoreNTierDbContext.Set<TEntity>().AsQueryable();

        if (includes is not null)
        {
            foreach (var item in includes)
            {
                en = en.Include(item);
            }
        }
        if (filter is not null)
        {
            en = en.Where(filter);
        }

        return en.ToPagedList(pageNumber, pageSize);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Update(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
    {
        _aspNetCoreNTierDbContext.UpdateRange(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

}
