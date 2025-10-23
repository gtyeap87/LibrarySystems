using MediatR;
using Library.Model;
using Library.Repository;
using Library.Repository.Specification;

namespace Library.Features.Queries;

public record GetFullBooksQuery(Genre? Genre, string? Name) : IRequest<IEnumerable<Book>>;

public class GetFullBooksQueryHandler(IQueryRepo<Book> queryRepo) : IRequestHandler<GetFullBooksQuery, IEnumerable<Book>>
{
    private readonly IQueryRepo<Book> _queryRepo = queryRepo;

    public async Task<IEnumerable<Book>> Handle(GetFullBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new FullBooksSpec(query.Genre, query.Name);
        return await _queryRepo.ListAsync(spec);
    }
}