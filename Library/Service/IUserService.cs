using Library.Model.Request;

namespace Library.Service
{
    public interface IUserService
    {
        Task DeleteAsync(DeleteUserRequest request);

        Task<string> LoginAsync(LoginUserRequest request);

        Task<Guid> RegisterAsync(RegisterUserRequest request);
    }
}