using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateBookStockCommand(Model.BookStock BookStock) : IRequest<Model.BookStock>;

    public class UpdateBookStockCommandHandler(ICommandRepo<Model.BookStock> bookStockCommandRepo) : IRequestHandler<UpdateBookStockCommand, Model.BookStock>
    {
        private readonly ICommandRepo<Model.BookStock> _memberCommandRepo = bookStockCommandRepo;

        public async Task<Model.BookStock> Handle(UpdateBookStockCommand command, CancellationToken cancellationToken)
        {
            return await _memberCommandRepo.UpdateAsync(command.BookStock);
        }
    }
}