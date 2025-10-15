using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBookCommand(Model.Book Book) : IRequest<Model.Book>;

    public class UpdateBookCommandHandler(ICommandRepo<Model.Book> memberCommandRepo) : IRequestHandler<UpdateBookCommand, Model.Book>
    {
        private readonly ICommandRepo<Model.Book> _memberCommandRepo = memberCommandRepo;

        public async Task<Model.Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {

            return  await _memberCommandRepo.UpdateAsync(command.Book);
        }
    }
}