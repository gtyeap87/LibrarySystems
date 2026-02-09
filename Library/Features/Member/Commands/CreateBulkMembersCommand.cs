using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands;

public record CreateBulkMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

public class CreateBulkMembersCommandHandler(
    ICommandRepo<Model.Member> memberCommandRepo
    ) : IRequestHandler<CreateBulkMembersCommand>
{
    private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;

    public async Task Handle(CreateBulkMembersCommand command, CancellationToken cancellationToken)
    {
        await _memberCommandRepo.BulkInsertAsync(command.Members);
    }
}