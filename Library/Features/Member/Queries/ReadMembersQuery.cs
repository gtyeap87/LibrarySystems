using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Member.Queries;

public record ReadMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page) : IRequest<IEnumerable<Model.Member>>;

public class GetMembersQueryHandler(IQueryRepo<Model.Member> queryRepo) : IRequestHandler<ReadMembersQuery, IEnumerable<Model.Member>>
{
    private readonly IQueryRepo<Model.Member> _queryRepo = queryRepo;

    public async Task<IEnumerable<Model.Member>> Handle(ReadMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersOnlySpec(command.Name, command.Date);
        return await _queryRepo.ListAsync(spec, command.Page);
    }
}