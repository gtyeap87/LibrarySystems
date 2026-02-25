using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands;

public record CreateBulkMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

public class CreateBulkMembersCommandHandler(
    ICommandRepo<Model.Member> memberCommandRepo
    ) : IRequestHandler<CreateBulkMembersCommand>
{
    public async Task Handle(CreateBulkMembersCommand command, CancellationToken cancellationToken)
    {
        await memberCommandRepo.BulkInsertAsync(command.Members);
    }
}