using Library.Model;
using Library.Repository;
using Library.Repository.Specification;
using MediatR;

namespace Library.Features.Queries;

public record GetFullLoanedBooksQuery(string? BookName, string? MemberName) : IRequest<IEnumerable<LoanBook>>;

public class GetFullLoanedBooksQueryHandler(IQueryRepo<LoanBook> queryRepo) : IRequestHandler<GetFullLoanedBooksQuery, IEnumerable<LoanBook>>
{
    private readonly IQueryRepo<LoanBook> _memberQueryRepo = queryRepo;

    public async Task<IEnumerable<LoanBook>> Handle(GetFullLoanedBooksQuery command, CancellationToken cancellationToken)
    {
        var spec = new FullLoanedBookSpec(command.BookName, command.MemberName);
        return await _memberQueryRepo.ListAsync(spec);
    }
}