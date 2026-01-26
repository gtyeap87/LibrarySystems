using Library.Model;
using Library.Model.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Queries;

public record ReadFullBooksQuery(Genre? Genre, string? Name, PaginationRequest Page) : IRequest<IEnumerable<Book>>;

public class GetFullBooksQueryHandler(IQueryRepo<Book> queryRepo) : IRequestHandler<ReadFullBooksQuery, IEnumerable<Book>>
{
    private readonly IQueryRepo<Book> _queryRepo = queryRepo;

    public async Task<IEnumerable<Book>> Handle(ReadFullBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new FullBooksSpec(query.Genre, query.Name);
        return await _queryRepo.ListAsync(spec, query.Page);
    }
}