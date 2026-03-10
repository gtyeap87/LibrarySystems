using Library.Api.Dto.Requests;
using Library.Api.Models;
using Library.Api.Repositories;
using Library.Api.Specifications;
using MediatR;

namespace Library.Api.Features.Book.Queries;

public record ReadBooksQuery(Genre? Genre, string? Name, PaginationRequestDto Page) : IRequest<IEnumerable<Models.Book>>;

public class GetBooksQueryHandler(IQueryRepo<Models.Book> queryRepo) : IRequestHandler<ReadBooksQuery, IEnumerable<Models.Book>>
{
    public async Task<IEnumerable<Models.Book>> Handle(ReadBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new BooksOnlySpec(query.Genre, query.Name);
        return await queryRepo.ListAsync(spec, query.Page);
    }
}