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
        ILibraryCommandRepository commandRepo,
        IQueryRepo<Member> memberQueryRepo,
        IQueryRepo<Book> bookQueryRepo,
        IQueryRepo<LoanBook> loanBookQueryRepo,
        ICommandRepo<Book> bookCommandRepo,
        IMediator mediator
        ) : ILibraryService
    {
        private readonly ILibraryQueryRepository _queryRepo = queryRepo;
        private readonly ILibraryCommandRepository _commandRepo = commandRepo;
        private readonly IQueryRepo<Book> _bookQueryRepo = bookQueryRepo;
        private readonly IQueryRepo<LoanBook> _loanBookQueryRepo = loanBookQueryRepo;
        private readonly IQueryRepo<Member> _memberQueryRepo = memberQueryRepo;
        private readonly ICommandRepo<Book> _bookCommandRepo = bookCommandRepo;
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
            if (_commandRepo is ILibraryCommandRepository)
            {
                var book = request.Book;
                var qty = request.Qty;
                var newBookId = await _commandRepo.AddBookAsync(book, qty);
                return newBookId;
            }
            else
            {
                throw new InvalidOperationException("The repository does not support add operations.");
            }
        }

        public async Task<Book> UpdateBookAsync(BookRequest request)
        {
            // Assuming you have a command repository for updates
            if (_commandRepo is ILibraryCommandRepository)
            {
                var book = request.Book;

                return await _commandRepo.UpdateBookAsync(book);
            }
            else
            {
                throw new InvalidOperationException("The repository does not support update operations.");
            }
        }

        public async Task DeleteBookAsync(Guid bookId)
        {
            var book = await _bookQueryRepo.GetByIdAsync(bookId) ?? throw new KeyNotFoundException($"Book with ID {bookId} not found.");

            //Before delete, check if there are any active loans for the book
            var spec = new LoanedBookSpec(book?.Name, null);
            var activeLoans = await _loanBookQueryRepo.ListAsync(spec);
            if (activeLoans.Any())
            {
                throw new InvalidOperationException("Cannot delete book with active loans.");
            }
            else
            {
                await _bookCommandRepo.DeleteAsync(book);
            }
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