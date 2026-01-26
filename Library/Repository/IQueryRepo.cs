using Library.Model.Request;
using Library.Specification;

namespace Library.Repository
{
    public interface IQueryRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, PaginationRequest page);
    }
}