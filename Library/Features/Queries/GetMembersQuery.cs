using Library.Model;
using Library.Repository;
using Library.Repository.Specification;
using MediatR;

namespace Library.Features.Queries;

public record GetMembersQuery(string? Name, DateOnly? Date) : IRequest<IEnumerable<Member>>;

public class GetMembersQueryHandler(IQueryRepo<Member> queryRepo) : IRequestHandler<GetMembersQuery, IEnumerable<Member>>
{
    private readonly IQueryRepo<Member> _queryRepo = queryRepo;

    public async Task<IEnumerable<Member>> Handle(GetMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersOnlySpec(command.Name, command.Date);
        return await _queryRepo.ListAsync(spec);
    }
}