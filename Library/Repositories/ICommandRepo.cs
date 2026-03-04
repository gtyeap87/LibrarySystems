using EFCore.BulkExtensions;
using Library.Factories;

namespace Library.Repositories
{
    public interface ICommandRepo<T> where T : class
    {
        Task<Guid> AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task BulkInsertAsync<K>(IEnumerable<K> entities) where K : class, IRoot;

        Task BulkUpdateAsync<K>(IEnumerable<K> entities, BulkConfig bulkConfig) where K : class, IRoot;

        Task DeleteAsync(T entity);

        Task<T> UpdateAsync(T entity);
    }
}