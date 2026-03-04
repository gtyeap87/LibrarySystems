using Library.Data.Identity;
using Library.Strategies;

namespace Library.Factory
{
    /// <summary>
    /// Factory implementation that maps roles to strategies
    /// Centralizes all role-to-strategy selection logic
    /// </summary>
    /// <param name="strategies">Collection of available member strategies</param>
    public class MemberStrategyFactory(IEnumerable<IMemberStrategy> strategies) : IMemberStrategyFactory
    {
        public IMemberStrategy CreateStrategy(string role)
        {
            var strategy = GetStrategyByRole(role);
            return strategy ?? throw new InvalidOperationException(
                $"No member strategy found for role {role}");
        }

        private IMemberStrategy? GetStrategyByRole(string role)
        {
            return role switch
            {
                Roles.Member => strategies.FirstOrDefault(s => s.GetType().Name == nameof(PremiumMember)),

                _ => null
            };
        }
    }
}