using AutoFixture;
using AutoFixture.AutoMoq;
using EFCore.BulkExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Library.Features.Commands;
using Library.Features.Queries;
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
            var validatorMock = _fixture.Freeze<Mock<IValidator<Member>>>();
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            // Generate mokc return result
            // Create a valid validation result
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            //Mock validator
            validatorMock
                .Setup(v => v.ValidateAsync(
                It.IsAny<Member>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

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
            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<Member>(m => m == member),
                It.IsAny<CancellationToken>()),
                Times.Once
                );

            Assert.NotEqual(Guid.Empty, newGuid);
            Assert.Equal(newGuid, result);
        }

        [Fact]
        public async Task Command_Handler_Create_Bulk_Members_Success()
        {
            // Arrange

            var validatorMock = _fixture.Freeze<Mock<IValidator<IEnumerable<Member>>>>();
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            var validationResult = new ValidationResult(new List<ValidationFailure>());

            validatorMock
                .Setup(v => v.ValidateAsync(
                It.IsAny<IEnumerable<Member>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

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
            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<IEnumerable<Member>>(m => m.All(members.Contains)),
                It.IsAny<CancellationToken>()),
                Times.Once
                );

            Assert.Null(exception);
        }

        [Fact]
        public async Task Command_Handler_Update_Member_Success()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<Member>>>();
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            var validationResult = new ValidationResult(new List<ValidationFailure>());

            validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<Member>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

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
            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<Member>(m => m == member),
                It.IsAny<CancellationToken>()),
                Times.Once
                );

            Assert.NotNull(updMember);
            updMember.Should().BeEquivalentTo(member);
        }

        [Fact]
        public async Task Command_Handler_Update_Bulk_Members_Success()
        {
            // Arrange
            var validatorMock = _fixture.Freeze<Mock<IValidator<IEnumerable<Member>>>>();
            var memberCommandRepoMock = _fixture.Freeze<Mock<ICommandRepo<Member>>>();

            var validationResult = new ValidationResult(new List<ValidationFailure>());

            validatorMock
             .Setup(v => v.ValidateAsync(
             It.IsAny<IEnumerable<Member>>(),
             It.IsAny<CancellationToken>()))
             .ReturnsAsync(validationResult);

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
            validatorMock.Verify(v => v.ValidateAsync(
                It.Is<IEnumerable<Member>>(m => m.All(members.Contains)),
                It.IsAny<CancellationToken>()),
                Times.Once
                );

            Assert.Null(exception);
        }
    }
}