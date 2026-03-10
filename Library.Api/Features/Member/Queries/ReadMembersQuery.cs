using Library.Api.Dto.Requests;
using Library.Api.Repositories;
using Library.Api.Specifications;
using MediatR;

namespace Library.Api.Features.Member.Queries;

public record ReadMembersQuery(string? Name, DateOnly? Date, PaginationRequestDto Page) : IRequest<IEnumerable<Models.Member>>;

public class GetMembersQueryHandler(IQueryRepo<Models.Member> queryRepo) : IRequestHandler<ReadMembersQuery, IEnumerable<Models.Member>>
{
    public async Task<IEnumerable<Models.Member>> Handle(ReadMembersQuery command, CancellationToken cancellationToken)
    {
        var spec = new MembersOnlySpec(command.Name, command.Date);
        return await queryRepo.ListAsync(spec, command.Page);
    }
}