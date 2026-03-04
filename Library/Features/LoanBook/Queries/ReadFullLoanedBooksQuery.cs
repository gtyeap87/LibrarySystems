using Library.Dto.Requests;
using Library.Repositories;
using Library.Specifications;
using MediatR;

namespace Library.Features.LoanBook.Queries;

public record ReadFullLoanedBooksQuery(string? BookName, string? MemberName, PaginationRequestDto Page)
    : IRequest<IEnumerable<Models.LoanBook>>;

public class GetFullLoanedBooksQueryHandler(IQueryRepo<Models.LoanBook> queryRepo)
    : IRequestHandler<ReadFullLoanedBooksQuery, IEnumerable<Models.LoanBook>>
{
    public async Task<IEnumerable<Models.LoanBook>> Handle(ReadFullLoanedBooksQuery command, CancellationToken cancellationToken)
    {
        var spec = new FullLoanedBookSpec(command.BookName, command.MemberName);
        return await queryRepo.ListAsync(spec, command.Page);
    }
}