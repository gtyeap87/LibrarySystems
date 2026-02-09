using Library.Dto.Request;
using Library.Repository;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBookCommand(BookRequest Request) : IRequest<Model.Book>;

    public class UpdateBookCommandHandler(
        ICommandRepo<Model.Book> bookCommandRepo
        ) : IRequestHandler<UpdateBookCommand, Model.Book>
    {
        private readonly ICommandRepo<Model.Book> _memberCommandRepo = bookCommandRepo;

        public async Task<Model.Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.Request.Book);
        }
    }
}