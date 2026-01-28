using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Library.Middleware
{
    public static class ExceptionHandler
    {
        public static async Task UseGeneralExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    if (exception is ValidationException validationException)
                    {
                        var errors = validationException.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray()
                            );

                        await context.Response.WriteAsJsonAsync(errors);
                        return;
                    }

                    if (exception is Exception ex)
                    {
                        //todo: It is still needed? Will need to test later
                        var logger = app.Services.GetRequiredService<ILogger>();
                        logger.LogError(ex, "Error occurred while adding new member");

                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                });
            });
        }
    }
}