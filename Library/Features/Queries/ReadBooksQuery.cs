using MediatR;
using Library.Model;
using Library.Repository;
using Library.Repository.Specification;
using Library.Model.Request;

namespace Library.Features.Queries;

public record ReadBooksQuery(Genre? Genre, string? Name, PaginationRequest Page) : IRequest<IEnumerable<Book>>;

public class GetBooksQueryHandler(IQueryRepo<Book> queryRepo) : IRequestHandler<ReadBooksQuery, IEnumerable<Book>>
{
    private readonly IQueryRepo<Book> _queryRepo = queryRepo;

    public async Task<IEnumerable<Book>> Handle(ReadBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new BooksOnlySpec(query.Genre, query.Name);
        return await _queryRepo.ListAsync(spec, query.Page);
    }
}