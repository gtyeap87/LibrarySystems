using Library.Model.Request;

namespace Library.Strategies
{
    public interface IMemberStrategy
    {
        Task<Guid> AddMemberAsync(RegisterUserRequest request);

        bool CanHandle(string role);
    }
}