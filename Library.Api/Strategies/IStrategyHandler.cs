using Library.Api.Dto.Requests;

namespace Library.Api.Strategies
{
    public interface IStrategyHandler
    {
        Task<Guid> HandleAsync(RegisterUserRequest request);
    }
}