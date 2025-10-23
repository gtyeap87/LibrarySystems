using MediatR;
using Library.Model;
using Library.Repository;
using Library.Repository.Specification;

namespace Library.Features.Queries;

public record GetBooksQuery(Genre? Genre, string? Name) : IRequest<IEnumerable<Book>>;

public class GetBooksQueryHandler(IQueryRepo<Book> queryRepo) : IRequestHandler<GetBooksQuery, IEnumerable<Book>>
{
    private readonly IQueryRepo<Book> _queryRepo = queryRepo;

    public async Task<IEnumerable<Book>> Handle(GetBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new BooksOnlySpec(query.Genre, query.Name);
        return await _queryRepo.ListAsync(spec);
    }
}