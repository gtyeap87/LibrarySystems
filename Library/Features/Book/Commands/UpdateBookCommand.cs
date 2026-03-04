using Library.Dto.Requests;
using Library.Repositories;
using MediatR;

namespace Library.Features.Book.Commands
{
    public record UpdateBookCommand(BookRequest Request) : IRequest<Models.Book>;

    public class UpdateBookCommandHandler(
        ICommandRepo<Models.Book> bookCommandRepo
        ) : IRequestHandler<UpdateBookCommand, Models.Book>
    {
        public async Task<Models.Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            return await bookCommandRepo.UpdateAsync(command.Request.Book);
        }
    }
}