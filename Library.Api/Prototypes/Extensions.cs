using FluentValidation;
using Library.Api.Dto;
using Library.Api.Helpers;

namespace Library.Api.Prototypes
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

                var membersDto = Csv.ReadFile<CsvMemberDto>(file);

                return Results.Ok(membersDto);
            })
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery();
        }
    }
}