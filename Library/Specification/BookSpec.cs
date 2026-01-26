using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Specification
{
    public class BooksOnlySpec(Genre? genre, string? name) : Specification<Book>
    {
        public override IQueryable<Book> Apply(IQueryable<Book> query)
        {
            if (genre.HasValue)
                query = query.Where(m => m.Genre == genre.Value);

            if (!string.IsNullOrWhiteSpace(name))
            {
                var pattern = $"%{name.Trim()}%";
                query = query.Where(m => EF.Functions.Like(m.Name, pattern));
            }

            query = query.OrderBy(b => b.Id);

            return query;
        }
    }
}