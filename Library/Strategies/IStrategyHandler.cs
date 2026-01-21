using Library.Model.Request;

namespace Library.Strategies
{
    public interface IStrategyHandler
    {
        Task<Guid> HandleAsync(RegisterUserRequest request);
    }
}