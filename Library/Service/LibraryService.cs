using Library.Commands.Member;
using Library.Features.Commands;
using Library.Model;
using Library.Model.Request;
using Library.Repository;
using Library.Repository.Specification;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using RestWebApi.Service;

namespace Library.Service
{
    public class LibraryService(
        ILibraryQueryRepository queryRepo,
        IQueryRepo<Member> memberQueryRepo,
        IMediator mediator
        ) : ILibraryService
    {
        private readonly ILibraryQueryRepository _queryRepo = queryRepo;
        private readonly IQueryRepo<Member> _memberQueryRepo = memberQueryRepo;
        private readonly IMediator _mediator = mediator;

        #region Library

        #endregion Library

        #region Book

        public async Task<IEnumerable<Book>> GetBooksAsync(Genre? genre, string? name)
        {
            return await _queryRepo.GetBooksAsync(genre, name);
        }

        public async Task<Guid> AddBookAsync(BookRequest request)
        {
            var book = request.Book;
            var qty = request.Qty;
            var newBookId = await _mediator.Send(new AddBookCommand(book, qty));
            return newBookId;
        }

        public async Task<Book> UpdateBookAsync(BookRequest request)
        {
            var updatedBook = await _mediator.Send(new UpdateBookCommand(request.Book));

            var existingBook = (await _queryRepo.GetBooksAsync(updatedBook.Genre, updatedBook.Name))
                .FirstOrDefault(b => b.Id == request.Book.Id) ?? throw new InvalidOperationException($"Book with ID {request.Book.Id} not found");

            var bookStock = existingBook.BookStocks.FirstOrDefault(bs => bs.BookId == updatedBook.Id) ?? throw new InvalidOperationException($"BookStock for Book ID {updatedBook.Id} not found");
            bookStock.Quantity = request.Qty;
            await _mediator.Send(new UpdateBookStockCommand(bookStock));

            return updatedBook;
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            await _mediator.Send(new DeleteBookCommand(bookId));
        }

        #endregion Book

        #region Member

        /// <summary>
        /// This is non specification version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Member>> GetMembersAsync(string? name, DateOnly? date, bool include = false)
        {
            return await _queryRepo.GetMembersAsync(name, date, include);
        }

        public async Task<IEnumerable<Member>> GetMembersOnlyAsync(string? name, DateOnly? date)
        {
            var spec = new MembersOnlySpec(name, date);
            return await _memberQueryRepo.ListAsync(spec);
        }

        public async Task<IEnumerable<Member>> GetMembersExtended2Async(string? name, DateOnly? date)
        {
            var spec = new MembersWithLoansSpec(name, date);
            return await _memberQueryRepo.ListAsync(spec);
        }

        //[Obsolete("Use Spec method() instead.")]
        //public async Task<Guid> AddMemberAsync(AddMemberRequest request)
        //{
        //    if (_queryRepo is ILibraryCommandRepository)
        //    {
        //        var member = request.Member;
        //        var newMemberId = await _commandRepo.AddMemberAsync(member);
        //        return newMemberId;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("The repository does not support add operations.");
        //    }
        //}

        public async Task<Guid> AddMemberAsync(MemberRequest request)
        {
            var member = request.Member;
            var newMemberId = await _mediator.Send(new AddMemberCommand(member));
            return newMemberId;
        }

        public async Task<Member> UpdateMemberAsync(MemberRequest request)
        {
            var updatedMember = await _mediator.Send(new UpdateMemberCommand(request.Member));
            return updatedMember;
        }

        public async Task DeleteMemberAsync(Guid memberId)
        {
            await _mediator.Send(new DeleteMemberCommand(memberId));
        }

        #endregion Member

        #region Loan Book

        public async Task<IEnumerable<LoanBook>> GetLoanBooksAsync(string? bookName, string? memberName)
        {
            return await _queryRepo.GetLoanBooksAsync(bookName, memberName);
        }

        public async Task<Guid> AddLoanBookAsync(LoanBookRequest request)
        {
            var loanBook = request.LoanBook;
            var newLoanedBookId = await _mediator.Send(new AddLoanedBookCommand(loanBook));
            return newLoanedBookId;
        }

        public async Task<LoanBook> UpdateLoanedBookReturnedDateAsync(LoanBookRequest request)
        {
            var updatedLoanedBook = await _mediator.Send(new UpdateLoanedBookCommand(request.LoanBook));
            return updatedLoanedBook;
        }

        #endregion Loan Book
    }
}