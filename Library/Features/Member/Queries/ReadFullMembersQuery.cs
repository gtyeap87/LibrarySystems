using Library.Dto.Requests;
using Library.Repositories;
using Library.Specifications;
using MediatR;

namespace Library.Features.Member.Queries;

public record ReadFullMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page)
    : IRequest<IEnumerable<Models.Member>>;

public class ReadFullMembersQueryHandler(IQueryRepo<Models.Member> memberQueryRepo)
    : IRequestHandler<ReadFullMembersQuery, IEnumerable<Models.Member>>
{
    public async Task<IEnumerable<Models.Member>> Handle(ReadFullMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersWithLoansSpec(command.Name, command.Date);
        return await memberQueryRepo.ListAsync(spec, command.Page);
    }
}