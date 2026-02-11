using AutoFixture;
using AutoFixture.AutoMoq;
using EFCore.BulkExtensions;
using FluentAssertions;
using Library.Dto.Request;
using Library.Features.Book.Commands;
using Library.Features.Member.Commands;
using Library.Features.Member.Queries;
using Library.Model;
using Library.Repository;
using Library.Service;
using MediatR;
using Moq;

namespace Library.Test
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
            DateOnly RandomJoinedDate()
            {
                var start = DateTime.UtcNow.AddYears(-5);
                var range = (DateTime.UtcNow - start).Days;

                return DateOnly.FromDateTime(start.AddDays(_fixture.Create<int>() % range));
            }

            var members = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.JoinedDate, RandomJoinedDate())
                .CreateMany(5);

            //Mock mediator
            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<ReadMembersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(members);

            var libService = _fixture.Create<LibraryService>();

            // Act
            var result = await libService.ReadMembersOnlyAsync(null, null, new Dto.Request.PaginationRequestDto());

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
            var newGuid = Guid.NewGuid();
            memberCommandRepoMock.Setup(x => x.AddAsync(
                It.IsAny<Member>()))
                .ReturnsAsync(newGuid);

            var handler = _fixture.Create<AddMemberCommandHandler>();

            DateOnly RandomJoinedDate()
            {
                var start = DateTime.UtcNow.AddYears(-5);
                var range = (DateTime.UtcNow - start).Days;

                return DateOnly.FromDateTime(start.AddDays(_fixture.Create<int>() % range));
            }

            var member = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.JoinedDate, RandomJoinedDate())
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
               .With(m => m.JoinedDate, RandomJoinedDate())
               .CreateMany(5);

            memberCommandRepoMock.Setup(x => x.BulkInsertAsync(
                It.IsAny<IEnumerable<Member>>()));

            var handler = _fixture.Create<CreateBulkMembersCommandHandler>();

            DateOnly RandomJoinedDate()
            {
                var start = DateTime.UtcNow.AddYears(-5);
                var range = (DateTime.UtcNow - start).Days;

                return DateOnly.FromDateTime(start.AddDays(_fixture.Create<int>() % range));
            }

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
               .With(m => m.JoinedDate, RandomJoinedDate())
               .CreateMany(5);

            memberCommandRepoMock.Setup(x => x.BulkUpdateAsync(
                It.IsAny<IEnumerable<Member>>(),
                //bulkconfig
                It.IsAny<BulkConfig>()
                ));

            var handler = _fixture.Create<UpdateBulkMembersCommandHandler>();

            DateOnly RandomJoinedDate()
            {
                var start = DateTime.UtcNow.AddYears(-5);
                var range = (DateTime.UtcNow - start).Days;

                return DateOnly.FromDateTime(start.AddDays(_fixture.Create<int>() % range));
            }

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
            var newBookId = Guid.NewGuid();
            bookCommandRepoMock.Setup(x => x.AddAsync(
                It.IsAny<Book>()))
                .ReturnsAsync(newBookId);

            bookStockCommandRepoMock.Setup(x => x.AddAsync(
               It.IsAny<BookStock>()));

            var book = _fixture.Build<Book>()
                .Without(b => b.BookStocks)
                .With(b => b.Id, newBookId)
                .Create();

            var bookRequest = new BookRequest
            {
                Book = book,
                Qty = 5
            };

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

            var booksRequest = books.Select(book => new BookRequest() { Book = book, Qty = _fixture.Build<int>().Create() });
            var req = new BooksRequest
            {
                Books = booksRequest
            };

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

            var bookId = Guid.NewGuid();
            var book = _fixture.Build<Book>()
                .Without(b => b.BookStocks)
                .With(b => b.Id, bookId)
                .Create();

            var bookRequest = new BookRequest
            {
                Book = book,
                Qty = 5
            };

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

            var booksRequest = books.Select(book => new BookRequest() { Book = book, Qty = _fixture.Build<int>().Create() });
            var req = new BooksRequest
            {
                Books = booksRequest
            };

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

        #endregion Loan Book
    }
}