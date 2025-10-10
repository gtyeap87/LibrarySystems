using Library.Data;
using Library.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.Repository
{
    //Make generic read repository
    public class EfQueryRepo<T>(LibraryContext context) : IQueryRepo<T> where T : class
    {
        private readonly LibraryContext _context = context;

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            var query = spec.Apply(_context.Set<T>().AsQueryable());
            return await query
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