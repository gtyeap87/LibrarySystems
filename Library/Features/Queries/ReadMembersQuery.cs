using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Queries;

public record ReadMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page) : IRequest<IEnumerable<Member>>;

public class GetMembersQueryHandler(IQueryRepo<Member> queryRepo) : IRequestHandler<ReadMembersQuery, IEnumerable<Member>>
{
    private readonly IQueryRepo<Member> _queryRepo = queryRepo;

    public async Task<IEnumerable<Member>> Handle(ReadMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersOnlySpec(command.Name, command.Date);
        return await _queryRepo.ListAsync(spec, command.Page);
    }
}