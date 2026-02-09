using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMembersCommand(IEnumerable<Model.Member> Members) : IRequest;

    public class AddMembersCommandHandler(
        ICommandRepo<Model.Member> commandRepo
        ) : IRequestHandler<CreateMembersCommand>
    {
        private readonly ICommandRepo<Model.Member> _commandRepo = commandRepo;

        public async Task Handle(CreateMembersCommand command, CancellationToken cancellationToken)
        {
            await _commandRepo.AddRangeAsync(command.Members);
        }
    }
}