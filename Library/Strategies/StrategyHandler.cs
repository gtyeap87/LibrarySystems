using Library.Model.Request;

namespace Library.Strategies
{
    /// <summary>
    /// Strategy handler to manage member strategies
    /// Business logic to select and execute appropriate member strategy based on role
    /// </summary>
    /// <param name="strategies"></param>
    public class StrategyHandler(IEnumerable<IMemberStrategy> strategies) : IStrategyHandler
    {
        private readonly IEnumerable<IMemberStrategy> _strategies = strategies;

        public async Task<Guid> HandleAsync(RegisterUserRequest request)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.Role))
                ?? throw new InvalidOperationException(
                    $"No member strategy found for role {request.Role}");

            return await strategy.AddMemberAsync(request);
        }
    }
}