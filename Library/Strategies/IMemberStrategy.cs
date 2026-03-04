using Library.Dto.Requests;

namespace Library.Strategies
{
    public interface IMemberStrategy
    {
        Task<Guid> AddMemberAsync(RegisterUserRequest request);
    }
}