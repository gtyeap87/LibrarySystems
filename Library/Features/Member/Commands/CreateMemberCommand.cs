using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record CreateMemberCommand(Model.Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo
        ) : IRequestHandler<CreateMemberCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var newId = await memberCommandRepo.AddAsync(command.Member);
            return newId;
        }
    }
}