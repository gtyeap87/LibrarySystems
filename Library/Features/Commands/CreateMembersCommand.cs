using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class AddMembersCommandHandler(ICommandRepo<Member> commandRepo) : IRequestHandler<CreateMembersCommand>
    {
        private readonly ICommandRepo<Member> _commandRepo = commandRepo;

        public async Task Handle(CreateMembersCommand command, CancellationToken cancellationToken)
        {
            await _commandRepo.AddRangeAsync(command.Members);
        }
    }
}