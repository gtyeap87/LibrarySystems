using Library.Dto.Request;

namespace Library.Strategies
{
    public interface IStrategyHandler
    {
        Task<Guid> HandleAsync(RegisterUserRequest request);
    }
}