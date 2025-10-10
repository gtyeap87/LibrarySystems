using Library.Model;
using Library.Repository.Specification;

namespace Library.Repository
{
    public interface ICommandRepo<T> where T : class
    {
        Task<Guid> AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task<T> UpdateAsync(T entity);
    }
}