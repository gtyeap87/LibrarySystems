using Library.Data.Identities;
using Library.Dto.Requests;

namespace Library.Services
{
    public interface IUserService
    {
        Task ChangePasswordAsync(ChangePasswordRequest request);

        Task DeleteUserAsync(Guid deleteUserId);

        Task<string> LoginUserAsync(LoginUserRequest request);

        Task<(ApplicationUser User, IList<string> Roles)> ReadUserAsync(Guid id);

        Task<Guid> RegisterUserAsync(RegisterUserRequest request);

        Task UpdateUserAsync(Guid id, UpdateUserRequest request);
    }
}