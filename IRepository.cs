using System.Linq.Expressions;

namespace Commerce.Core;

public interface IRepository<T> where T : IEntity
{
    Task CreateAsync(T entity);

    Task<IReadOnlyCollection<T>> GetListAsync();

    Task<IReadOnlyCollection<T>> GetListAsync(Expression<Func<T, bool>> filter);

    Task<T> GetAsync(Guid id);

    Task<T> GetAsync(Expression<Func<T, bool>> filter);

    Task RemoveAsync(Guid id);

    Task UpdateAsync(T entity);
}