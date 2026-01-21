using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record CreateMemberCommand(Member Member) : IRequest<Guid>;

    public class AddMemberCommandHandler(ICommandRepo<Member> memberCommandRepo) : IRequestHandler<CreateMemberCommand, Guid>
    {
        private readonly ICommandRepo<Member> _memberCommandRepo = memberCommandRepo;

        public async Task<Guid> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.AddAsync(command.Member);
        }
    }
}