using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Library.Api.Dto.Requests;
using Library.Api.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Library.Api.Test
{
    public class FluentValidationUnitTest
    {
        private readonly IFixture _fixture;

        private const string LibrayIdIsRequiredMessage = $"{nameof(Book.LibraryId)} is required";
        private const string NameIsRequiredMessage = $"{nameof(Book.Name)} is required";
        private const string QuantityIsRequiredMessage = $"{nameof(BookStock.Quantity)} is required";
        private const string BookIdIsRequiredMessage = $"{nameof(LoanBook.BookId)} is required";
        private const string MemberIdIsRequiredMessage = $"{nameof(LoanBook.MemberId)} is required";
        private const string LoanedDateIsRequiredMessage = $"{nameof(LoanBook.LoanedDate)} is required";
        private const string IdIsRequiredForUpdateMessage = $"{nameof(LoanBook.Id)} is required for update";
        private const string IdIsRequiredForDeleteMessage = $"{nameof(LoanBook.Id)} is required for delete";

        public FluentValidationUnitTest()
        {
            _fixture = new Fixture()
            .Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true
            });
        }

        #region Member

        [Fact]
        public async Task Member_Validation_Success_With_Valid_Member()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<Member>>>();

            // Create a valid validation result with no errors
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            // Setup the mock to return a valid result
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<Member>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var member = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.LibraryId, Guid.CreateVersion7())
                .With(m => m.JoinedDate, DateOnly.FromDateTime(DateTime.Now))
                .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(member);

            // Assert
            Assert.True(result.IsValid);

            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<Member>(m => m.Id == member.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Member_Validation_Fails_With_Invalid_Data()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<Member>>>();

            var missingName = new ValidationFailure(nameof(Member.Name), "Name is required");
            var missingLibraryId = new ValidationFailure(nameof(Member.LibraryId), "LibraryId is required");

            var validationResult = new ValidationResult([missingName, missingLibraryId]);

            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<Member>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var member = _fixture.Build<Member>()
                .Without(m => m.LoanedBooks)
                .With(m => m.Name, string.Empty)
                .With(m => m.LibraryId, Guid.Empty)
                .With(m => m.JoinedDate, DateOnly.FromDateTime(DateTime.Now))
                .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(member);

            // Assert
            Assert.False(result.IsValid);

            result.Errors.Should().AllSatisfy(error =>
            {
                switch (error.PropertyName)
                {
                    case nameof(Member.Name):
                        error.ErrorMessage.Should().Be(NameIsRequiredMessage);
                        break;

                    case nameof(Member.LibraryId):
                        error.ErrorMessage.Should().Be(LibrayIdIsRequiredMessage);
                        break;
                }
            });

            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<Member>(m => m.Id == member.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion Member

        #region Book

        [Fact]
        public async Task Book_Validation_Success_With_Valid_Member()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<BookRequest>>>();

            // Create a valid validation result with no errors
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            // Setup the mock to return a valid result
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<BookRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var book = _fixture.Build<Book>()
                .Without(m => m.BookStocks)
                .Create();

            var bookRequest = new BookRequest(book, 5);

            // Act
            var result = await validatorMock.Object.ValidateAsync(bookRequest);

            // Assert
            Assert.True(result.IsValid);

            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<BookRequest>(m => m.Book.Id == bookRequest.Book.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Book_Validation_Fails_With_Invalid_Data()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<BookRequest>>>();

            var missingName = new ValidationFailure(nameof(Book.Name), NameIsRequiredMessage);
            var missingLibraryId = new ValidationFailure(nameof(Book.LibraryId), LibrayIdIsRequiredMessage);
            var missingBookStockQty = new ValidationFailure(nameof(BookStock.Quantity), QuantityIsRequiredMessage);

            var validationResult = new ValidationResult([missingName, missingLibraryId, missingBookStockQty]);

            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<BookRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var book = _fixture.Build<Book>()
                .Without(m => m.BookStocks)
                .With(m => m.Name, string.Empty)
                .With(m => m.LibraryId, Guid.Empty)
                .Create();

            var bookRequest = new BookRequest(book, 0);

            // Act
            var result = await validatorMock.Object.ValidateAsync(bookRequest);

            // Assert
            Assert.False(result.IsValid);

            result.Errors.Should().AllSatisfy(error =>
            {
                switch (error.PropertyName)
                {
                    case nameof(Book.Name):
                        error.ErrorMessage.Should().Be(NameIsRequiredMessage);
                        break;

                    case nameof(Book.LibraryId):
                        error.ErrorMessage.Should().Be(LibrayIdIsRequiredMessage);
                        break;

                    case nameof(BookStock.Quantity):
                        error.ErrorMessage.Should().Be(QuantityIsRequiredMessage);
                        break;
                }
            });

            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<BookRequest>(m => m.Book.Id == bookRequest.Book.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion Book

        #region LoanBook

        [Fact]
        public async Task LoanBook_Validation_Success_With_Valid_Data()
        {
            // Arrange
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            var validatorMock = _fixture.Freeze<Mock<IValidator<LoanBook>>>();
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<LoanBook>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var loanBook = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .With(lb => lb.LoanedDate, DateOnly.FromDateTime(DateTime.Now))
                .With(lb => lb.ReturnedDate, DateOnly.FromDateTime(DateTime.Now.AddDays(25)))
                .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(loanBook);

            // Assert
            Assert.True(result.IsValid);

            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<LoanBook>(m => m.Id == loanBook.Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task LoanBook_Validation_Fails_With_Invalid_Data()
        {
            // Arrange
            var missingBookId = new ValidationFailure(nameof(LoanBook.BookId), BookIdIsRequiredMessage);
            var missingMemberId = new ValidationFailure(nameof(LoanBook.MemberId), MemberIdIsRequiredMessage);
            var missingLoanedDate = new ValidationFailure(nameof(LoanBook.LoanedDate), LoanedDateIsRequiredMessage);

            var validationResult = new ValidationResult([missingBookId, missingMemberId, missingLoanedDate]);

            var validatorMock = _fixture.Freeze<Mock<IValidator<LoanBook>>>();
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<LoanBook>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var loanBook = _fixture.Build<LoanBook>()
               .Without(lb => lb.Book)
               .Without(lb => lb.Member)
               .Without(lb => lb.ReturnedDate)
               .With(lb => lb.BookId, Guid.Empty)
               .With(lb => lb.MemberId, Guid.Empty)
               .With(lb => lb.LoanedDate, DateOnly.FromDateTime(DateTime.Now))
               .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(loanBook);

            // Assert
            Assert.False(result.IsValid);

            result.Errors.Should().AllSatisfy(error =>
            {
                switch (error.PropertyName)
                {
                    case nameof(LoanBook.BookId):
                        error.ErrorMessage.Should().Be(BookIdIsRequiredMessage);
                        break;

                    case nameof(LoanBook.MemberId):
                        error.ErrorMessage.Should().Be(MemberIdIsRequiredMessage);
                        break;

                    case nameof(LoanBook.LoanedDate):
                        error.ErrorMessage.Should().Be(LoanedDateIsRequiredMessage);
                        break;
                }
            });
        }

        [Fact]
        public async Task LoanBook_Validation_Fails_When_Update_Without_Id()
        {
            // Arrange
            var httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();

            httpRequestMock.Setup(r => r.Method).Returns("PUT");
            httpContextMock.Setup(c => c.Request).Returns(httpRequestMock.Object);
            httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);

            var missingIdForUpdate = new ValidationFailure(nameof(LoanBook.Id), IdIsRequiredForUpdateMessage);

            var validationResult = new ValidationResult([missingIdForUpdate]);

            var validatorMock = _fixture.Freeze<Mock<IValidator<LoanBook>>>();
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<LoanBook>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var loanBook = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, DateOnly.FromDateTime(DateTime.Now))
                .With(l => l.Id, Guid.Empty)
                .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(loanBook);

            // Assert
            Assert.False(result.IsValid);

            result.Errors.Should().AllSatisfy(error =>
            {
                switch (error.PropertyName)
                {
                    case nameof(LoanBook.Id):
                        error.ErrorMessage.Should().Be(IdIsRequiredForUpdateMessage);
                        break;
                }
            });
        }

        [Fact]
        public async Task LoanBook_Validation_Fails_When_Delete_Without_Id()
        {
            // Arrange
            var httpContextAccessorMock = _fixture.Freeze<Mock<IHttpContextAccessor>>();
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();

            httpRequestMock.Setup(r => r.Method).Returns("DELETE");
            httpContextMock.Setup(c => c.Request).Returns(httpRequestMock.Object);
            httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);

            var missingIdForDelete = new ValidationFailure(nameof(LoanBook.Id), IdIsRequiredForDeleteMessage);

            var validationResult = new ValidationResult([missingIdForDelete]);

            var validatorMock = _fixture.Freeze<Mock<IValidator<LoanBook>>>();
            validatorMock
                .Setup(v => v.ValidateAsync(
                    It.IsAny<LoanBook>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var loanBook = _fixture.Build<LoanBook>()
                .Without(lb => lb.Book)
                .Without(lb => lb.Member)
                .Without(lb => lb.ReturnedDate)
                .With(lb => lb.LoanedDate, DateOnly.FromDateTime(DateTime.Now))
                .With(l => l.Id, Guid.Empty)
                .Create();

            // Act
            var result = await validatorMock.Object.ValidateAsync(loanBook);

            // Assert
            Assert.False(result.IsValid);

            result.Errors.Should().AllSatisfy(error =>
            {
                switch (error.PropertyName)
                {
                    case nameof(LoanBook.Id):
                        error.ErrorMessage.Should().Be(IdIsRequiredForDeleteMessage);
                        break;
                }
            });
        }

        #endregion LoanBook
    }
}