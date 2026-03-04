using Library.Data.Contexts;
using Library.Dto.Requests;
using Library.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
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