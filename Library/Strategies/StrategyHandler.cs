using Library.Dto.Request;

namespace Library.Strategies
{
    /// <summary>
    /// Strategy handler to manage member strategies
    /// Business logic to execute appropriate member strategy based on role
    /// </summary>
    /// <param name="strategyFactory">Factory for creating member strategies</param>
    public class StrategyHandler(IMemberStrategyFactory factory) : IStrategyHandler
    {
        public async Task<Guid> HandleAsync(RegisterUserRequest request)
        {
            var strategy = factory.CreateStrategy(request.Role);
            return await strategy.AddMemberAsync(request);
        }
    }
}