using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Member.Queries;

public record ReadFullMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page)
    : IRequest<IEnumerable<Model.Member>>;

public class ReadFullMembersQueryHandler(IQueryRepo<Model.Member> memberQueryRepo)
    : IRequestHandler<ReadFullMembersQuery, IEnumerable<Model.Member>>
{
    public async Task<IEnumerable<Model.Member>> Handle(ReadFullMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersWithLoansSpec(command.Name, command.Date);
        return await memberQueryRepo.ListAsync(spec, command.Page);
    }
}