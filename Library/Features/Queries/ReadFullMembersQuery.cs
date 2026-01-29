using Library.Dto.Request;
using Library.Model;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Queries;

public record ReadFullMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page) : IRequest<IEnumerable<Member>>;

public class GetFullMembersQueryHandler(IQueryRepo<Member> memberQueryRepo) : IRequestHandler<ReadFullMembersQuery, IEnumerable<Member>>
{
    private readonly IQueryRepo<Member> _memberQueryRepo = memberQueryRepo;

    public async Task<IEnumerable<Member>> Handle(ReadFullMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersWithLoansSpec(command.Name, command.Date);
        return await _memberQueryRepo.ListAsync(spec, command.Page);
    }
}