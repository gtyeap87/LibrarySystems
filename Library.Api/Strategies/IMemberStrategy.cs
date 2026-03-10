using Library.Api.Dto.Requests;

namespace Library.Api.Strategies
{
    public interface IMemberStrategy
    {
        Task<Guid> AddMemberAsync(RegisterUserRequest request);
    }
}