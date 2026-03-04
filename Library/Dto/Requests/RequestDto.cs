using Library.Data.Identities;
using Library.Models;

namespace Library.Dto.Requests
{
    public record BookRequest(Book Book, int Qty);

    public record BooksRequest(IEnumerable<BookRequest> Books);

    public record MemberRequest(Member Member);

    public record MembersRequest(IEnumerable<MemberRequest> Members);

    public record LoanBookRequest(LoanBook LoanBook);

    public record LoanBooksRequest(IEnumerable<LoanBook> LoanBooks);

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