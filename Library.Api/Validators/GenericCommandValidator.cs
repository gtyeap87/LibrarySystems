using FluentValidation;
using MediatR;
using System.Linq.Expressions;

namespace Library.Api.Validators;

/// <summary>
/// Generic validator for commands that return a response and contain a DTO property to validate.
/// </summary>
internal sealed class GenericCommandValidator<TCommand, TRequest, TResponse> : AbstractValidator<TCommand>
    where TCommand : class, IRequest<TResponse>
{
    public GenericCommandValidator(
        IValidator<TRequest> dtoValidator,
        Expression<Func<TCommand, TRequest>> propertySelector)
    {
        RuleFor(propertySelector).SetValidator(dtoValidator);
    }
}

/// <summary>
/// Generic validator for commands that don't return a response and contain a DTO property to validate.
/// </summary>
internal sealed class GenericCommandValidator<TCommand, TRequest> : AbstractValidator<TCommand>
    where TCommand : class, IRequest
{
    public GenericCommandValidator(
        IValidator<TRequest> dtoValidator,
        Expression<Func<TCommand, TRequest>> propertySelector)
    {
        RuleFor(propertySelector).SetValidator(dtoValidator);
    }
}