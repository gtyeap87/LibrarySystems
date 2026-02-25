using Library.Dto.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.LoanBook.Queries;

public record ReadFullLoanedBooksQuery(string? BookName, string? MemberName, PaginationRequestDto Page)
    : IRequest<IEnumerable<Model.LoanBook>>;

public class GetFullLoanedBooksQueryHandler(IQueryRepo<Model.LoanBook> queryRepo)
    : IRequestHandler<ReadFullLoanedBooksQuery, IEnumerable<Model.LoanBook>>
{
    public async Task<IEnumerable<Model.LoanBook>> Handle(ReadFullLoanedBooksQuery command, CancellationToken cancellationToken)
    {
        var spec = new FullLoanedBookSpec(command.BookName, command.MemberName);
        return await queryRepo.ListAsync(spec, command.Page);
    }
}