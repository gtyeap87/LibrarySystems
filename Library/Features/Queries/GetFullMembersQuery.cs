using Library.Model;
using Library.Repository;
using Library.Repository.Specification;
using MediatR;

namespace Library.Features.Queries;

public record GetFullMembersQuery(string? Name, DateOnly? Date) : IRequest<IEnumerable<Member>>;

public class GetFullMembersQueryHandler(IQueryRepo<Member> memberQueryRepo) : IRequestHandler<GetFullMembersQuery, IEnumerable<Member>>
{
    private readonly IQueryRepo<Member> _memberQueryRepo = memberQueryRepo;

    public async Task<IEnumerable<Member>> Handle(GetFullMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersWithLoansSpec(command.Name, command.Date);
        return await _memberQueryRepo.ListAsync(spec);
    }
}