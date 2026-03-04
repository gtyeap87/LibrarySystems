using Library.Dto.Requests;
using Library.Models;
using Library.Repositories;
using Library.Specifications;
using MediatR;

namespace Library.Features.Book.Queries;

public record ReadBooksQuery(Genre? Genre, string? Name, PaginationRequestDto Page) : IRequest<IEnumerable<Models.Book>>;

public class GetBooksQueryHandler(IQueryRepo<Models.Book> queryRepo) : IRequestHandler<ReadBooksQuery, IEnumerable<Models.Book>>
{
    public async Task<IEnumerable<Models.Book>> Handle(ReadBooksQuery query, CancellationToken cancellationToken)
    {
        var spec = new BooksOnlySpec(query.Genre, query.Name);
        return await queryRepo.ListAsync(spec, query.Page);
    }
}