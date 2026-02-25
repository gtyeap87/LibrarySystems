using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class AddMembersCommandHandler(
        ICommandRepo<Model.Member> commandRepo
        ) : IRequestHandler<CreateMembersCommand>
    {
        public async Task Handle(CreateMembersCommand command, CancellationToken cancellationToken)
        {
            await commandRepo.AddRangeAsync(command.Members);
        }
    }
}