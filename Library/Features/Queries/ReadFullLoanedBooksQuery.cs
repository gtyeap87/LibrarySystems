using Library.Model;
using Library.Model.Request;
using Library.Repository;
using Library.Specification;
using MediatR;

namespace Library.Features.Queries;

public record ReadFullLoanedBooksQuery(string? BookName, string? MemberName, PaginationRequest Page) : IRequest<IEnumerable<LoanBook>>;

public class GetFullLoanedBooksQueryHandler(IQueryRepo<LoanBook> queryRepo) : IRequestHandler<ReadFullLoanedBooksQuery, IEnumerable<LoanBook>>
{
    private readonly IQueryRepo<LoanBook> _memberQueryRepo = queryRepo;

    public async Task<IEnumerable<LoanBook>> Handle(ReadFullLoanedBooksQuery command, CancellationToken cancellationToken)
    {
        var spec = new FullLoanedBookSpec(command.BookName, command.MemberName);
        return await _memberQueryRepo.ListAsync(spec, command.Page);
    }
}