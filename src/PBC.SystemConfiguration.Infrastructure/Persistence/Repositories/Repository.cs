using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;

namespace PBC.SystemConfiguration.Infrastructure.Persistence.Repositories;

public class Repository<T>(ProgramDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> DbSet = context.Set<T>();
    
    /// <summary>
    /// Asynchronously retrieves all entities of type <typeparamref name="T"/> from the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of all entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously retrieves an entity of type <typeparamref name="T"/> by filter.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a filtered and paginated list of entities.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="offset">The number of entities to skip.</param>
    /// <param name="limit">The maximum number of entities to return.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="List{T}"/> of entities matching the criteria.</returns>
    /// <remarks>
    /// This method applies the <paramref name="predicate"/> first, then skips <paramref name="offset"/> rows, and takes <paramref name="limit"/> rows.
    /// </remarks>
    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, int offset = 0, int limit = 100, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(predicate).Skip(offset).Take(limit).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously adds a new entity of type <typeparamref name="T"/> to the context.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously adds a collection of new entities of type <typeparamref name="T"/> to the context.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Marks the specified entity as modified in the context.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <remarks>
    /// This method does not persist changes to the database immediately; <c>SaveChangesAsync</c> must be called on the context.
    /// </remarks>
    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    /// <summary>
    /// Marks the specified entity for deletion in the context.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <remarks>
    /// This method does not delete the entity from the database immediately; <c>SaveChangesAsync</c> must be called on the context.
    /// </remarks>
    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
    
    /// <summary>
    /// Marks a collection of entities for deletion in the context.
    /// </summary>
    /// <param name="entities">The collection of entities to remove.</param>
    /// <remarks>
    /// This method does not delete the entities from the database immediately; <c>SaveChangesAsync</c> must be called on the context.
    /// </remarks>
    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Asynchronously determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">An expression to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if any elements in the source sequence pass the test in the specified predicate; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }
    
    /// <summary>
    /// Persists all pending changes in the current context to the underlying data store asynchronously.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}