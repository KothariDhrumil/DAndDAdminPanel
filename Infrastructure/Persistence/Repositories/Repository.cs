using System.Linq.Expressions;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository implementation with optional in-memory caching for read operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly IMemoryCache? _cache;
    private readonly bool _enableCaching;
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(10);

    public Repository(DbContext context, IMemoryCache? cache = null, bool enableCaching = false)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _cache = cache;
        _enableCaching = enableCaching && cache != null;
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{typeof(T).Name}_{id}";

        if (_enableCaching && _cache != null)
        {
            if (_cache.TryGetValue(cacheKey, out T? cachedEntity))
            {
                return cachedEntity;
            }
        }

        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);

        if (_enableCaching && _cache != null && entity != null)
        {
            _cache.Set(cacheKey, entity, DefaultCacheDuration);
        }

        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{typeof(T).Name}_All";

        if (_enableCaching && _cache != null)
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<T>? cachedEntities))
            {
                return cachedEntities!;
            }
        }

        var entities = await _dbSet.ToListAsync(cancellationToken);

        if (_enableCaching && _cache != null)
        {
            _cache.Set(cacheKey, entities, DefaultCacheDuration);
        }

        return entities;
    }

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        InvalidateCache();
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        InvalidateCache();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        InvalidateCache();
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
        InvalidateCache();
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        InvalidateCache();
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    /// <summary>
    /// Invalidates the cache for this entity type
    /// </summary>
    protected virtual void InvalidateCache()
    {
        if (_enableCaching && _cache != null)
        {
            var cacheKeyPrefix = typeof(T).Name;
            _cache.Remove($"{cacheKeyPrefix}_All");
        }
    }
}
