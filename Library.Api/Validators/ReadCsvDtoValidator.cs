using FluentValidation;

namespace Library.Api.Validators
{
    internal sealed class ReadCsvDtoValidator : AbstractValidator<IFormFile>
    {
        private static readonly string[] allowedTypes =
        {
            "text/csv",
            "application/csv",
            "application/vnd.ms-excel"
        };

        public ReadCsvDtoValidator()
        {
            RuleFor(file => file).NotNull()
                .WithMessage("not valid file");

            RuleFor(file => file)
                .Must(file =>
                allowedTypes.Any(t => t.Equals(file.ContentType, StringComparison.OrdinalIgnoreCase)) &&
                Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only CSV files are allowed");
        }
    }
}