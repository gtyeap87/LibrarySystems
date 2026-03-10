using Library.Api.Repositories;
using MediatR;

namespace Library.Api.Features.Member.Commands;

public record CreateBulkMembersCommand(IEnumerable<Models.Member> Members) : IRequest;

public class CreateBulkMembersCommandHandler(
    ICommandRepo<Models.Member> memberCommandRepo
    ) : IRequestHandler<CreateBulkMembersCommand>
{
    public async Task Handle(CreateBulkMembersCommand command, CancellationToken cancellationToken)
    {
        await memberCommandRepo.BulkInsertAsync(command.Members);
    }
}