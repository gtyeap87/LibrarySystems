using Library.Model;

namespace Library.Repository.Specification
{
    public class BooksOnlySpec(Genre? genre, string? name) : Specification<Book>
    {
        private readonly string? _name = name;
        private readonly Genre? _genre = genre;

        public override IQueryable<Book> Apply(IQueryable<Book> query)
        {
            if (_genre.HasValue)
                query = query.Where(m => m.Genre == _genre.Value);

            if (!string.IsNullOrEmpty(_name))
                query = query.Where(m => m.Name.ToLower() == _name.ToLower());

            return query;
        }
    }
}