using Library.Repositories;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMembersCommand(IEnumerable<Models.Member> Members) : IRequest;

    public class AddMembersCommandHandler(
        ICommandRepo<Models.Member> commandRepo
        ) : IRequestHandler<CreateMembersCommand>
    {
        public async Task Handle(CreateMembersCommand command, CancellationToken cancellationToken)
        {
            await commandRepo.AddRangeAsync(command.Members);
        }
    }
}