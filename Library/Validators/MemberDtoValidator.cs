using FluentValidation;
using Library.Model;

namespace Library.Validators
{
    internal sealed class MemberDtoValidator2 : AbstractValidator<Member>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemberDtoValidator2(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // Always validate these
            RuleFor(x => x.LibraryId).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(Member.LibraryId)} is required");

            RuleFor(x => x.Name).NotNull().NotEmpty()
                .WithMessage($"{nameof(Member.Name)} is required");

            RuleFor(x => x.JoinedDate).NotEqual(DateOnly.MinValue)
                .WithMessage($"{nameof(Member.JoinedDate)} is required");

            // Only validate Id for updates
            RuleFor(x => x.Id).NotEqual(Guid.Empty)
                .WithMessage($"{nameof(Member.Id)} is required for update ")
                .When(IsUpdate);

            RuleFor(x => x.Id).NotEqual(Guid.Empty)
               .WithMessage($"{nameof(Member.Id)} is required for delete")
               .When(IsDelete);
        }

        private bool IsUpdate(Member request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "PUT" or "PATCH";
        }

        private bool IsDelete(Member request)
        {
            var method = _httpContextAccessor.HttpContext?.Request.Method;
            return method is "DELETE";
        }
    }

    internal sealed class MembersDtoValidator2 : AbstractValidator<IEnumerable<Member>>
    {
        public MembersDtoValidator2(IHttpContextAccessor httpContextAccessor)
        {
            RuleForEach(members => members).SetValidator(new MemberDtoValidator2(httpContextAccessor));
        }
    }
}