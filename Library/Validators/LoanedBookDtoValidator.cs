using FluentValidation;
using Library.Models;

namespace Library.Validators
{
    internal sealed class LoanBookDtoValidator : AbstractValidator<LoanBook>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoanBookDtoValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // Always validate these
            RuleFor(x => x).NotNull();

            RuleFor(x => x.BookId).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(LoanBook.BookId)} is required");

            RuleFor(x => x.MemberId).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(LoanBook.MemberId)} is required");

            RuleFor(x => x.LoanedDate).NotEqual(DateOnly.MinValue)
                .WithMessage($"{nameof(LoanBook.LoanedDate)} is required");

            // Only validate Id for updates
            RuleFor(x => x.Id).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(LoanBook.Id)} is required for update ")
                .When(IsUpdate);

            RuleFor(x => x.Id).NotEqual(Guid.Empty)
               .WithMessage($"{nameof(LoanBook.Id)} is required for delete")
               .When(IsDelete);
        }

        private bool IsUpdate(LoanBook request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "PUT" or "PATCH";
        }

        private bool IsDelete(LoanBook request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "DELETE";
        }
    }

    internal sealed class LoanBooksDtoValidator : AbstractValidator<IEnumerable<LoanBook>>
    {
        public LoanBooksDtoValidator(IHttpContextAccessor httpContextAccessor)
        {
            RuleForEach(loanBooks => loanBooks).SetValidator(new LoanBookDtoValidator(httpContextAccessor));
        }
    }
}