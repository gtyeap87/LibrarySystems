using FluentValidation;
using Library.Model;
using Library.Repository;
using MediatR;

namespace Library.Features.Commands
{
    public record UpdateLoanedBookCommand(LoanBook LoanBook) : IRequest<LoanBook>;

    public class UpdateLoanedBookCommandHandler(
        ICommandRepo<LoanBook> memberCommandRepo,
        IValidator<LoanBook> validator
        ) : IRequestHandler<UpdateLoanedBookCommand, LoanBook>
    {
        private readonly ICommandRepo<LoanBook> _memberCommandRepo = memberCommandRepo;
        private readonly IValidator<LoanBook> _validator = validator;

        public async Task<LoanBook> Handle(UpdateLoanedBookCommand command, CancellationToken cancellationToken)
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