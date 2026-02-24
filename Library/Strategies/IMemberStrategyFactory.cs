namespace Library.Strategies
{
    /// <summary>
    /// Factory interface for creating member strategies based on role
    /// </summary>
    public interface IMemberStrategyFactory
    {
        IMemberStrategy CreateStrategy(string role);
    }
}