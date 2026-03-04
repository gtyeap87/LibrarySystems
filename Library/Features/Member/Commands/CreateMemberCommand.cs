using Library.Repositories;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMemberCommand(Models.Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler(
        ICommandRepo<Models.Member> memberCommandRepo
        ) : IRequestHandler<CreateMemberCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var newId = await memberCommandRepo.AddAsync(command.Member);
            return newId;
        }
    }
}