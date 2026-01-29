using FluentValidation;
using Library.Dto;

namespace Library.Prototypes
{
    public static class Extensions
    {
        public static async Task SetUpTestMinimalApi(this WebApplication app)
        {
            app.MapPost("/api/test/read-csv/member", async (IFormFile file, IValidator<IFormFile> validator) =>
            {
                var result = await validator.ValidateAsync(file);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors);
                }

                var membersDto = Helpers.Csv.ReadFile<CsvMemberDto>(file);

                return Results.Ok(membersDto);
            })
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery();
        }
    }
}