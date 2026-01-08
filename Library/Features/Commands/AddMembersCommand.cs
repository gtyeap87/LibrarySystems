using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record AddMembersCommand(IEnumerable<Member> Members) : IRequest;

    public class AddMembersCommandHandler(ICommandRepo<Member> commandRepo) : IRequestHandler<AddMembersCommand>
    {
        private readonly ICommandRepo<Member> _commandRepo = commandRepo;

        public async Task Handle(AddMembersCommand command, CancellationToken cancellationToken)
        {
            await _commandRepo.AddRangeAsync(command.Members);
        }
    }
}