using Library.Repository;
using MediatR;

namespace Library.Features.Member.Commands
{
    public record UpdateMemberCommand(Model.Member Member) : IRequest<Model.Member>;

    public class UpdateMemberCommandHandler(
        ICommandRepo<Model.Member> memberCommandRepo
        ) : IRequestHandler<UpdateMemberCommand, Model.Member>
    {
        private readonly ICommandRepo<Model.Member> _memberCommandRepo = memberCommandRepo;

        public async Task<Model.Member> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.Member);
        }
    }
}