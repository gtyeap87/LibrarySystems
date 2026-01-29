using Library.Data;
using Library.Dto.Request;
using Library.Specification;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class EfQueryRepo<T>(LibraryContext context) : IQueryRepo<T> where T : class
    {
        private readonly LibraryContext _context = context;

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec, PaginationRequestDto page)
        {
            var query = spec.Apply(_context.Set<T>().AsQueryable());

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
            return await _context.Set<T>().FindAsync(id);
        }
    }
}