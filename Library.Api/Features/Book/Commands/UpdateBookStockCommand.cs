using Library.Api.Models;
using Library.Api.Repositories;
using MediatR;

namespace Library.Api.Features.Book.Commands
{
    public record UpdateBookStockCommand(BookStock BookStock) : IRequest<BookStock>;

    public class UpdateBookStockCommandHandler(
        ICommandRepo<BookStock> bookStockCommandRepo
        ) : IRequestHandler<UpdateBookStockCommand, BookStock>
    {
        public async Task<BookStock> Handle(UpdateBookStockCommand command, CancellationToken cancellationToken)
        {
            return await bookStockCommandRepo.UpdateAsync(command.BookStock);
        }
    }
}