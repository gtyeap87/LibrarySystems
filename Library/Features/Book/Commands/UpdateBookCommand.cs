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
        public async Task<Model.Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            return await bookCommandRepo.UpdateAsync(command.Request.Book);
        }
    }
}