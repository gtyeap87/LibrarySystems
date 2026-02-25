using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateMemberCommand(Model.Member Member) : IRequest<Model.Member>;

    public class UpdateMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo
        ) : IRequestHandler<UpdateMemberCommand, Model.Member>
    {
        public async Task<Model.Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
        {
            return await memberCommandRepo.UpdateAsync(command.Member);
        }
    }
}