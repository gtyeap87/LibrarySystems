using AutoFixture;
using AutoFixture.AutoMoq;
using EFCore.BulkExtensions;
using FluentAssertions;
using Library.Api.Dto.Requests;
using Library.Api.Features.Book.Commands;
using Library.Api.Features.LoanBook.Commands;
using Library.Api.Features.Member.Commands;
using Library.Api.Features.Member.Queries;
using Library.Api.Models;
using Library.Api.Repositories;
using Library.Api.Services;
using MediatR;
using Moq;

namespace Library.Api.Test
{
    public class LibraryUnitTest
    {
        private readonly IFixture _fixture;

        public LibraryUnitTest()
        {
            _fixture = new Fixture()
            .Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true
            });
        }

        #region Member

        /// <summary>
        /// This is just example for writing read unit test. But it is not recommended to write read unit test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Liibrary_Service_Read_Members_No_Filtering_Return_Success()
        {
            // Arrange

            // Get the auto-created mock
            var mediatorMock = _fixture.Freeze<Mock<IMediator>>();

            // Generate mokc return result

            var members = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.JoinedDate, RandomDate())
                .CreateMany(5);

            //Mock mediator
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<ReadMembersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(members);

            var libService = _fixture.Create<LibraryService>();

            // Act
            var result = await libService.ReadMembersOnlyAsync(null, null, new PaginationRequestDto());

            // Assert
            Assert.NotNull(result);
            await Assert.AllAsync(result, async member =>
            {
                Assert.NotEqual(Guid.Empty, member.Id);
                Assert.NotEqual(Guid.Empty, member.LibraryId);
                Assert.False(string.IsNullOrWhiteSpace(member.Name));
            });
        }

        [Fact]
        public async Task Command_Handler_Create_Member_Success()
        {
            // Arrange

            // Get the auto-created mock
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            // Mock command repo
            var newGuid = Guid.CreateVersion7();
            memberCommandRepoMock.Setup(x => x.AddAsync(
                It.IsAny<Member>()))
                .ReturnsAsync(newGuid);

            var handler = _fixture.Create<AddMemberCommandHandler>();

            var member = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.JoinedDate, RandomDate())
                .Create();

            var command = new CreateMemberCommand(member);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotEqual(Guid.Empty, newGuid);
            Assert.Equal(newGuid, result);
        }

        [Fact]
        public async Task Command_Handler_Create_Bulk_Members_Success()
        {
            // Arrange

            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            var members = _fixture.Build<Member>()
               .Without(m => m.LoanedBooks)
               .With(m => m.JoinedDate, RandomDate())
               .CreateMany(5);

            memberCommandRepoMock.Setup(x => x.BulkInsertAsync(
                It.IsAny<IEnumerable<Member>>()));

            var handler = _fixture.Create<CreateBulkMembersCommandHandler>();

            var command = new CreateBulkMembersCommand(members);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task Command_Handler_Update_Member_Success()
        {
            // Arrange
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            static DateOnly FixedJoinedDate()
            {
                var start = DateTime.UtcNow.AddYears(-5);
                var range = (DateTime.UtcNow - start).Days;

                return DateOnly.FromDateTime(start.AddDays(10 % range));
            }
            var member = _fixture.Build<Member>()
               .Without(m => m.LoanedBooks)
               .With(m => m.JoinedDate, FixedJoinedDate())
               .Create();

            memberCommandRepoMock.Setup(x => x.UpdateAsync(
                It.IsAny<Member>()))
                .ReturnsAsync(member);

            var handler = _fixture.Create<UpdateMemberCommandHandler>();

            var command = new UpdateMemberCommand(member);

            // Act
            var updMember = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotNull(updMember);
            updMember.Should().BeEquivalentTo(member);
        }

        [Fact]
        public async Task Command_Handler_Update_Bulk_Members_Success()
        {
            // Arrange
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            var members = _fixture.Build<Member>()
               .Without(m => m.LoanedBooks)
               .With(m => m.JoinedDate, RandomDate())
               .CreateMany(5);

            memberCommandRepoMock.Setup(x => x.BulkUpdateAsync(
                It.IsAny<IEnumerable<Member>>(),
                //bulkconfig
                It.IsAny<BulkConfig>()
                ));

            var handler = _fixture.Create<UpdateBulkMembersCommandHandler>();

            var command = new UpdateBulkMembersCommand(members);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert

            Assert.Null(exception);
        }

        #endregion Member

        #region Book

        [Fact]
        public async Task Command_Handler_Create_Book_Success()
        {
            // Arrange

            // Get the auto-created mock
            var bookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Book>>>();
            var bookStockCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<BookStock>>>();

            // Mock command repo
            var newBookId = Guid.CreateVersion7();
            bookCommandRepoMock.Setup(x => x.AddAsync(
                It.IsAny<Book>()))
                .ReturnsAsync(newBookId);

            bookStockCommandRepoMock.Setup(x => x.AddAsync(
               It.IsAny<BookStock>()));

            var book = _fixture.Build<Book>()
                .Without(b => b.BookStocks)
                .With(b => b.Id, newBookId)
                .Create();

            var bookRequest = new BookRequest(book, 5);

            var handler = _fixture.Create<CreateBookCommandHandler>();
            var command = new CreateBookCommand(bookRequest);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotEqual(Guid.Empty, newBookId);
            Assert.Equal(newBookId, result);
        }

        [Fact]
        public async Task Command_Handler_Create_Bulk_Books_Success()
        {
            // Arrange

            var bookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Book>>>();
            var bookStockCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<BookStock>>>();

            var books = _fixture.Build<Book>()
               .Without(m => m.BookStocks)
               .CreateMany(5);

            var booksRequest = books.Select(book => new BookRequest(book, _fixture.Build<int>().Create()));
            var req = new BooksRequest(booksRequest);

            bookCommandRepoMock.Setup(x => x.BulkInsertAsync(
                It.IsAny<IEnumerable<Book>>()));
            bookStockCommandRepoMock.Setup(x => x.BulkInsertAsync(
                It.IsAny<IEnumerable<BookStock>>()));

            var handler = _fixture.Create<CreateBulkBooksCommandHandler>();
            var command = new CreateBulkBooksCommand(req);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task Command_Handler_Update_Book_Success()
        {
            // Arrange
            var bookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Book>>>();

            var bookId = Guid.CreateVersion7();
            var book = _fixture.Build<Book>()
                .Without(b => b.BookStocks)
                .With(b => b.Id, bookId)
                .Create();

            var bookRequest = new BookRequest(book, 5);

            bookCommandRepoMock.Setup(x => x.UpdateAsync(
                It.IsAny<Book>()))
                .ReturnsAsync(book);

            var handler = _fixture.Create<UpdateBookCommandHandler>();
            var command = new UpdateBookCommand(bookRequest);

            // Act
            var updBook = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotNull(updBook);
            updBook.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async Task Command_Handler_Update_Bulk_Books_Success()
        {
            // Arrange

            var bookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Book>>>();
            var bookStockCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<BookStock>>>();

            var books = _fixture.Build<Book>()
               .Without(m => m.BookStocks)
               .CreateMany(5);

            var booksRequest = books.Select(book => new BookRequest(book, _fixture.Build<int>().Create()));
            var req = new BooksRequest(booksRequest);

            bookCommandRepoMock.Setup(x => x.BulkUpdateAsync(
                It.IsAny<IEnumerable<Book>>(),
                It.IsAny<BulkConfig>()
                ));
            bookStockCommandRepoMock.Setup(x => x.BulkUpdateAsync(
                It.IsAny<IEnumerable<BookStock>>(),
                It.IsAny<BulkConfig>()
                ));

            var handler = _fixture.Create<UpdateBulkBooksCommandHandler>();
            var command = new UpdateBulkBooksCommand(req);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert

            Assert.Null(exception);
        }

        #endregion Book

        #region Loan Book

        [Fact]
        public async Task Command_Handler_Create_Loaned_Book_Success()
        {
            // Arrange

            // Get the auto-created mock
            var loanBookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<LoanBook>>>();

            // Mock command repo

            var newId = Guid.CreateVersion7();

            loanBookCommandRepoMock.Setup(x => x.AddAsync(
                It.IsAny<LoanBook>()))
                .ReturnsAsync(newId);

            var loanBook = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, RandomDate())
                .With(lb => lb.Id, newId)
                .Create();

            var handler = _fixture.Create<AddLoanedBookCommandHandler>();
            var command = new CreateLoanedBookCommand(loanBook);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotEqual(Guid.Empty, newId);
            Assert.Equal(newId, result);
        }

        [Fact]
        public async Task Command_Handler_Create_Bulk_Loaned_Books_Success()
        {
            // Arrange

            var loanBookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<LoanBook>>>();

            var books = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, RandomDate())
                .CreateMany(5);

            loanBookCommandRepoMock.Setup(x => x.BulkInsertAsync(
                It.IsAny<IEnumerable<LoanBook>>()));

            var handler = _fixture.Create<CreateBulkLoanBooksCommandHandler>();
            var command = new CreateBulkLoanBooksCommand(books);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task Command_Handler_Update_Loan_Book_Success()
        {
            // Arrange
            var loanBookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<LoanBook>>>();

            var id = Guid.CreateVersion7();
            var loanBook = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, RandomDate())
                .With(lb => lb.Id, id)
                .Create();

            loanBookCommandRepoMock.Setup(x => x.UpdateAsync(
                It.IsAny<LoanBook>()))
                .ReturnsAsync(loanBook);

            var handler = _fixture.Create<UpdateLoanedBookCommandHandler>();
            var command = new UpdateLoanedBookCommand(loanBook);

            // Act
            var updLoanBook = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.NotNull(updLoanBook);
            updLoanBook.Should().BeEquivalentTo(loanBook);
        }

        [Fact]
        public async Task Command_Handler_Update_Bulk_Loan_Books_Success()
        {
            // Arrange

            var loanBookCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<LoanBook>>>();

            var loanBooks = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, RandomDate())
                .CreateMany(5);

            loanBookCommandRepoMock.Setup(x => x.BulkUpdateAsync(
                It.IsAny<IEnumerable<LoanBook>>(),
                It.IsAny<BulkConfig>()
                ));

            var handler = _fixture.Create<UpdateBulkLoanBooksCommandHandler>();
            var command = new UpdateBulkLoanBooksCommand(loanBooks);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

            // Assert

            Assert.Null(exception);
        }

        #endregion Loan Book

        private DateOnly RandomDate()
        {
            var start = DateTime.UtcNow.AddYears(-5);
            var range = (DateTime.UtcNow - start).Days;
            return DateOnly.FromDateTime(start.AddDays(_fixture.Create<int>() % range));
        }
    }
}