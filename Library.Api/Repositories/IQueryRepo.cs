using Library.Api.Dto.Requests;
using Library.Api.Specifications;

namespace Library.Api.Repositories
{
    public interface IQueryRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, PaginationRequestDto page);
    }
}