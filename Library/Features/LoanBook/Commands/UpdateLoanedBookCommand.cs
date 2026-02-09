using FluentValidation;
using Library.Repository;
using MediatR;

namespace Library.Features.LoanBook.Commands
{
    public record UpdateLoanedBookCommand(Model.LoanBook LoanBook) : IRequest<Model.LoanBook>;

    public class UpdateLoanedBookCommandHandler(
        ICommandRepo<Model.LoanBook> memberCommandRepo,
        IValidator<Model.LoanBook> validator
        ) : IRequestHandler<UpdateLoanedBookCommand, Model.LoanBook>
    {
        private readonly ICommandRepo<Model.LoanBook> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<Model.LoanBook> _validator = validator;

        public async Task<Model.LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
        {
            var loanBook = command.LoanBook;

            var result = await _validator.ValidateAsync(loanBook, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await _memberCommandRepo.UpdateAsync(loanBook);
        }
    }
}