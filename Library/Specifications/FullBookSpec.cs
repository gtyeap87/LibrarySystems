using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Specifications
{
    public class FullBooksSpec(Genre? genre, string? name) : BooksOnlySpec(genre, name)
    {
        public override IQueryable<Book> Apply(IQueryable<Book> query)
        {
            // Start with base filters
            query = base.Apply(query);

            // Add include
            query = query.Include(m => m.BookStocks);

            return query;
        }
    }
}