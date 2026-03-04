using Library.Dto.Requests;
using Library.Specifications;

namespace Library.Repositories
{
    public interface IQueryRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, PaginationRequestDto page);
    }
}