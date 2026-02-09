using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Book.Queries;

public record ReadFullBooksQuery(Genre? Genre, string? Name, PaginationRequestDto Page) : IRequest<IEnumerable<Model.Book>>;

public class GetFullBooksQueryHandler(IQueryRepo<Model.Book> queryRepo) : IRequestHandler<ReadFullBooksQuery, IEnumerable<Model.Book>>
{
    private readonly IQueryRepo<Model.Book> _queryRepo = queryRepo;

    public async Task<IEnumerable<Model.Book>> Handle(ReadFullBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new FullBooksSpec(query.Genre, query.Name);
        return await _queryRepo.ListAsync(spec, query.Page);
    }
}