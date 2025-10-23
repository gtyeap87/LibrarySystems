using MediatR;
using Library.Model;
using Library.Repository;

namespace Library.Features.Queries;

public record GetBooksCommand(Genre? Genre, string? Name) : IRequest<IEnumerable<Book>>;

public class GetBooksQueryHandler(ILibraryQueryRepository queryRepo) : IRequestHandler<GetBooksCommand, IEnumerable<Book>>
{
    private readonly ILibraryQueryRepository _queryRepo = queryRepo;

    public async Task<IEnumerable<Book>> Handle(GetBooksCommand query, CancellationToken cancellationToken)
    {
        return await _queryRepo.GetBooksAsync(query.Genre, query.Name);
    }
}