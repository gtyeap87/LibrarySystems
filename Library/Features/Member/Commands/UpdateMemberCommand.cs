using Library.Repositories;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateMemberCommand(Models.Member Member) : IRequest<Models.Member>;

    public class UpdateMemberCommandHandler(
        ICommandRepo<Models.Member> memberCommandRepo
        ) : IRequestHandler<UpdateMemberCommand, Models.Member>
    {
        public async Task<Models.Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
        {
            return await memberCommandRepo.UpdateAsync(command.Member);
        }
    }
}