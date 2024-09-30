using System.Linq.Expressions;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KALS.Repository.Implement;

public class GenericRepository<T>: IGenericRepository<T>, IAsyncDisposable where T : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<T> _dbSet;
    
    public GenericRepository(DbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<T>();
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }

    #region Get
    public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _dbSet;
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) return orderBy(query).AsNoTracking().FirstOrDefaultAsync();
        
        return query.AsNoTracking().FirstOrDefaultAsync();
    }

    public Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _dbSet;
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) return orderBy(query).AsNoTracking().Select(selector).FirstOrDefaultAsync();
        
        return query.AsNoTracking().Select(selector).FirstOrDefaultAsync();
    }

    public Task<IPaginate<TResult>> GetPagingListAsync<TResult>(Expression<Func<T, TResult>> selector, IFilter<T> filter, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int page = 1, int size = 10, string? sortBy = null, bool isAsc = true)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            var filterExpression = filter.ToExpression();
            query = query.Where(filterExpression);
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);
            
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new[] {typeof(T), property.Type}, query.Expression, Expression.Quote(lambda));
            query = query.Provider.CreateQuery<T>(resultExpression);
        }
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) return orderBy(query).AsNoTracking().Select(selector).ToPaginateAsync(page, size, 1);
        return query.AsNoTracking().Select(selector).ToPaginateAsync(page, size, 1);
    }

    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _dbSet;
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) return await orderBy(query).AsNoTracking().ToListAsync();

        return await query.AsNoTracking().ToListAsync();
    }
    #endregion

    #region Insert
    public async Task InsertAsync(T entity)
    {
        if (entity == null) return;
        await _dbSet.AddAsync(entity);
    }

    public Task InsertRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null) return Task.CompletedTask;
        return _dbSet.AddRangeAsync(entities);
    }
    #endregion

    #region Update
    public void UpdateAsync(T entity)
    {
        if(entity == null) return;
        _dbSet.Update(entity);
    }

    public void UpdateRangeAsync(IEnumerable<T> entities)
    {
        if(entities == null) return;
        _dbSet.UpdateRange(entities);
    }

    public void DeleteAsync(T entity)
    {
        if(entity == null) return;
        _dbSet.Remove(entity);
    }

    #endregion
    
}