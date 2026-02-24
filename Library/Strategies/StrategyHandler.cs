using Library.Dto.Request;

namespace Library.Strategies
{
    /// <summary>
    /// Strategy handler to manage member strategies
    /// Business logic to execute appropriate member strategy based on role
    /// </summary>
    /// <param name="strategyFactory">Factory for creating member strategies</param>
    public class StrategyHandler(IMemberStrategyFactory strategyFactory) : IStrategyHandler
    {
        private readonly IMemberStrategyFactory _strategyFactory = strategyFactory;

        public async Task<Guid> HandleAsync(RegisterUserRequest request)
        {
            var strategy = _strategyFactory.CreateStrategy(request.Role);
            return await strategy.AddMemberAsync(request);
        }
    }
}