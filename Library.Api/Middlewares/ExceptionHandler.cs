using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Library.Api.Middlewares
{
    public static class ExceptionHandler
    {
        public static async Task UseGeneralExceptionHandler(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

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

                    if (exception is InvalidOperationException ioex)
                    {
                        logger.LogError(ioex, "Not found");

                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync(ioex.Message);
                        return;
                    }

                    if (exception is Exception ex)
                    {
                        logger.LogError(ex, "Unexpected error");

                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                });
            });
        }
    }
}