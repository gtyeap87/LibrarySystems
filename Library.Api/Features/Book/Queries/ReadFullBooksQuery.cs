using Library.Api.Dto.Requests;
using Library.Api.Models;
using Library.Api.Repositories;
using Library.Api.Specifications;
using MediatR;

namespace Library.Api.Features.Book.Queries;

public record ReadFullBooksQuery(Genre? Genre, string? Name, PaginationRequestDto Page) : IRequest<IEnumerable<Models.Book>>;

public class GetFullBooksQueryHandler(IQueryRepo<Models.Book> queryRepo) : IRequestHandler<ReadFullBooksQuery, IEnumerable<Models.Book>>
{
    public async Task<IEnumerable<Models.Book>> Handle(ReadFullBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new FullBooksSpec(query.Genre, query.Name);
        return await queryRepo.ListAsync(spec, query.Page);
    }
}