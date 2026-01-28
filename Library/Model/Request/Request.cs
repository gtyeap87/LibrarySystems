using Library.Data.Identity;

namespace Library.Model.Request
{
    public class BookRequest
    {
        public required Book Book { get; set; }
        public int Qty { get; set; }
    }

    public class BooksRequest
    {
        public required IEnumerable<BookRequest> Books { get; set; } = [];
    }

    public class MemberRequest
    {
        public required Member Member { get; set; }
    }

    public class MembersRequest
    {
        public required IEnumerable<MemberRequest> Members { get; set; }
    }

    public class LoanBookRequest
    {
        public required LoanBook LoanBook { get; set; }
    }

    public class LoanBooksRequest
    {
        public required IEnumerable<LoanBook> LoanBooks { get; set; }
    }

    public record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Initials,
        string Password,
        bool EnableNotification = false,
        bool TwoFactorAuthentication = false,
        string Role = Roles.Librarian
        );

    public record DeleteUserRequest(
        string LoginId,
        string DeleteId
        );

    public record LoginUserRequest(
        string Email,
        string Password
        );

    public record UpdateUserRequest(
        string? FirstName,
        string? SecondName,
        string? Initials,
        bool? EnableNotification,
        bool? TwoFactorAuthentication,
        string? PhoneNumber,
        bool? LockoutEnabled,
        bool? LockoutEnd
        );

    public record ChangePasswordRequest(
        string Email,
        string Password,
        string NewPassword
        ) : LoginUserRequest(Email, Password);
}