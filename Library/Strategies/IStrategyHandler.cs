using Library.Dto.Requests;

namespace Library.Strategies
{
    public interface IStrategyHandler
    {
        Task<Guid> HandleAsync(RegisterUserRequest request);
    }
}