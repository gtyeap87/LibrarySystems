using Library.Dto.Requests;
using Library.Models;
using Library.Repositories;
using Library.Specifications;
using MediatR;

namespace Library.Features.Book.Queries;

public record ReadFullBooksQuery(Genre? Genre, string? Name, PaginationRequestDto Page) : IRequest<IEnumerable<Models.Book>>;

public class GetFullBooksQueryHandler(IQueryRepo<Models.Book> queryRepo) : IRequestHandler<ReadFullBooksQuery, IEnumerable<Models.Book>>
{
    public async Task<IEnumerable<Models.Book>> Handle(ReadFullBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new FullBooksSpec(query.Genre, query.Name);
        return await queryRepo.ListAsync(spec, query.Page);
    }
}