using Library.Api.Data.Contexts;
using Library.Api.Dto.Requests;
using Library.Api.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories
{
    public class EfQueryRepo<T>(LibraryContext context) : IQueryRepo<T> where T : class
    {
        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, PaginationRequestDto page)
        {
            var query = spec.Apply(context.Set<T>().AsQueryable());

            var skipCount = (page.PageNumber - 1) * page.PageSize;

            return await query
                .Skip(skipCount)
                .Take(page.PageSize)
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }
    }
}