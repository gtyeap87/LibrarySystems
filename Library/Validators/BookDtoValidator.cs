using FluentValidation;
using Library.Model;
using Library.Model.Request;

namespace Library.Validators
{
    internal sealed class BookDtoValidator : AbstractValidator<BookRequest>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookDtoValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // Always validate these
            RuleFor(x => x.Book.LibraryId).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(Book.LibraryId)} is required");

            RuleFor(x => x.Book.Name).NotNull().NotEmpty()
                .WithMessage($"{nameof(Book.Name)} is required");

            RuleFor(x => x.Book.Genre).NotNull()
                .WithMessage($"{nameof(Book.Genre)} is required");

            // Only validate Id for updates
            RuleFor(x => x.Book.Id).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(Book.Id)} is required for update ")
                .When(IsUpdate);

            RuleFor(x => x.Book.Id).NotEqual(Guid.Empty)
               .WithMessage($"{nameof(Book.Id)} is required for delete")
               .When(IsDelete);

            RuleFor(x => x.Qty).NotEqual(0).NotNull().NotEmpty()
                .WithMessage($"{nameof(BookStock.Quantity)} is required");
        }

        private bool IsUpdate(BookRequest request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "PUT" or "PATCH";
        }

        private bool IsDelete(BookRequest request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "DELETE";
        }
    }

    internal sealed class BooksDtoValidator : AbstractValidator<BooksRequest>
    {
        public BooksDtoValidator(IHttpContextAccessor httpContextAccessor)
        {
            RuleForEach(req => req.Books).SetValidator(new BookDtoValidator(httpContextAccessor));
        }
    }

    #region Book Stock

    internal sealed class BookStockDtoValidator : AbstractValidator<BookStock>
    {
        public BookStockDtoValidator()
        {
            // Always validate these
            RuleFor(x => x.Quantity).NotEqual(0)
                .WithMessage($"{nameof(BookStock.Quantity)} is required and cannot be less or equal to zero");
        }
    }

    #endregion Book Stock
}