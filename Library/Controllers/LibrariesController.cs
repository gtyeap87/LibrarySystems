using Asp.Versioning;
using Library.Authorization;
using Library.Dto;
using Library.Dto.Requests;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/libraries")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class LibrariesController(
        ILogger<LibrariesController> logger,
        ILibraryService service) : ControllerBase
    {
        #region Book

        /// <summary>
        /// Get Books
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet("books")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        [RequirePermission(Permissions.ReadBooks)]
        public async Task<IActionResult> ReadBooksAsync([FromQuery] Genre? genre, string? name, int pageSize, int pageNumber)
        {
            try
            {
                logger.LogInformation("GetBooksAsync");
                var books = await service.ReadFullBooksAsync(genre, name, new PaginationRequestDto { PageSize = pageSize, PageNumber = pageNumber });
                var bookDtos = books.Select(b => new BookDto
                (
                    b.Genre,
                    b.Name,
                    AvailableQuantity: b.BookStocks.Sum(bs => bs.Quantity)
                ));

                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while reading books");
                return BadRequest();
            }
        }

        [HttpPost("book")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateBooks)]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookRequest request)
        {
            logger.LogInformation("Add new book to library");

            var newId = await service.CreateBookAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Added {Qty} new book name {Name} with ID: {Id}", request.Qty, request.Book.Name, newId);

            return StatusCode(StatusCodes.Status201Created, newId);
        }

        /// <summary>
        /// Added new books add range method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("books")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateBooks)]
        public async Task<IActionResult> CreateBooksAsync([FromBody] BooksRequest request)
        {
            logger.LogInformation("Add new books to library");

            await service.CreateBooksAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
            {
                foreach (var book in request.Books)
                {
                    logger.LogInformation("Added new book name {Name}", book.Book.Name);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Add new books bulk insert method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("books")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateBooks)]
        public async Task<IActionResult> CreateBulkBooksAsync([FromBody] BooksRequest request)
        {
            logger.LogInformation("Add new books to library");

            await service.CreateBulkBooksAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
            {
                foreach (var book in request.Books)
                {
                    logger.LogInformation("Added new book name {Name}", book.Book.Name);
                }
            }

            return NoContent();
        }

        [HttpPatch("book")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateBooks)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] BookRequest request)
        {
            logger.LogInformation("update book information");

            var newId = await service.UpdateBookAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Update {Qty} new book name {Name} with ID: {Id}", request.Qty, request.Book.Name, newId);

            return NoContent();
        }

        [HttpPut("books")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateBooks)]
        public async Task<IActionResult> UpdateBulkBooksAsync([FromBody] BooksRequest request)
        {
            logger.LogInformation("update books information");

            if (request == null)
                return BadRequest("no patch data.");

            await service.UpdateBulkBooksAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Books updated");

            return NoContent();
        }

        [HttpDelete("book/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.DeleteBooks)]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            try
            {
                logger.LogInformation("delete book information");

                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                await service.DeleteBookAsync(id);

                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Deleted book with ID: {Id}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting book");
                return BadRequest();
            }
        }

        #endregion Book

        #region Member

        /// <summary>
        /// Get Members
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("members")]
        [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
        [RequirePermission(Permissions.ReadMembers)]
        public async Task<IActionResult> ReadMembersAsync([FromQuery] string? name, DateOnly? date, int pageSize, int pageNumber)
        {
            logger.LogInformation("GetMembersAsync");

            var members2 = await service.ReadMembersOnlyAsync(name, date, new PaginationRequestDto { PageSize = pageSize, PageNumber = pageNumber });

            var membersDto = members2.Select(m => new MemberDto
            (
                m.Name,
                m.JoinedDate
            ));

            return Ok(membersDto);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("member")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateMembers)]
        public async Task<IActionResult> CreateMemberAsync(MemberRequest request)
        {
            logger.LogInformation("Add new member to library");
            var newId = await service.CreateMemberAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Added new member name {Name} with ID: {Id}", request.Member.Name, newId);

            return StatusCode(StatusCodes.Status201Created, newId);
        }

        /// <summary>
        /// Add multiple members add range method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("members")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateMembers)]
        public async Task<IActionResult> CreateMembersAsync(MembersRequest request)
        {
            logger.LogInformation("Add new members to library");

            await service.CreateMembersAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
            {
                foreach (var req in request.Members)
                {
                    logger.LogInformation("Added new member name {Name}", req.Member.Name);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Add multiple members bulk insert method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("members")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateMembers)]
        public async Task<IActionResult> CreateBulkMembersAsync(MembersRequest request)
        {
            logger.LogInformation("Add new members to library");

            await service.CreateBulkMembersAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
            {
                foreach (var req in request.Members)
                {
                    logger.LogInformation("Added new member name {Name}", req.Member.Name);
                }
            }

            return NoContent();
        }

        [HttpPatch("member/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateMembers)]
        public async Task<IActionResult> UpdateMemberAsync(Guid id, [FromBody] MemberRequest request)
        {
            logger.LogInformation("update member information");

            var updatedMember = await service.UpdateMemberAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Update member name {Name} with ID: {Id}", updatedMember.Name, id);
            }

            return NoContent();
        }

        [HttpPut("members")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateMembers)]
        public async Task<IActionResult> UpdateBulkMembersAsync([FromBody] MembersRequest request)
        {
            logger.LogInformation("update members information");

            if (request == null)
                return BadRequest("no patch data.");

            await service.UpdateBulkMembersAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Members updated");

            return NoContent();
        }

        [HttpDelete("member/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.DeleteMembers)]
        public async Task<IActionResult> DeleteMemberAsync(Guid id)
        {
            // Validator is not needed since it is only id checking
            if (id == Guid.Empty)
                return BadRequest("Id is required.");

            logger.LogInformation("delete member information");

            await service.DeleteMemberAsync(id);

            LogDeleted(id);

            return NoContent();
        }

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Information,
            Message = "Deleted ID: {Id}")]
        private partial void LogDeleted(Guid id);

        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Information,
            Message = "Updated ID: {Id}")]
        private partial void LogUpdated(Guid id);

        #endregion Member

        #region Loan Books

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("loan-books")]
        [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
        [RequirePermission(Permissions.ReadLoanBooks)]
        public async Task<IActionResult> ReadLoanBooksAsync([FromQuery] string? bookName, string? memberName, int pageSize, int pageNumber)
        {
            try
            {
                logger.LogInformation("GetLoanBooksAsync");
                var loanBooks = await service.ReadLoanBooksAsync(bookName, memberName, new PaginationRequestDto { PageSize = pageSize, PageNumber = pageNumber });
                var loanBookDtos = loanBooks.Select(lb => new LoanBookDto
                (
                    lb?.Book?.Name!,
                    lb?.Member?.Name!,
                    lb.LoanedDate,
                    lb.ReturnedDate
                ));

                var loanBooksDetailsDto = new LoanBooksDetailsDto
                (
                    loanBookDtos,
                    ReadLoanedOutBooksQuantity(loanBooks)
                );
                return Ok(loanBooksDetailsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while read loan books");
                return BadRequest();
            }
        }

        private static int ReadLoanedOutBooksQuantity(IEnumerable<LoanBook> loanBooks)
        {
            var totalQty = loanBooks.Count(lb => lb.ReturnedDate == null);
            return totalQty;
        }

        [HttpPost("loan-book")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateLoanBooks)]
        public async Task<IActionResult> CreateLoanBookAsync(LoanBookRequest request)
        {
            logger.LogInformation("Add new loan book record");
            var newId = await service.CreateLoanBookAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Added new loan book record with ID: {Id}", newId);

            return StatusCode(StatusCodes.Status201Created, newId);
        }

        [HttpPost("loan-books")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.CreateLoanBooks)]
        public async Task<IActionResult> CreateBulkLoanBooksAsync(LoanBooksRequest request)
        {
            logger.LogInformation("Add new loan books record");

            await service.CreateBulkLoanBooksAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Added new loan books");

            return NoContent();
        }

        [HttpPatch("loan-book")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateLoanBooks)]
        public async Task<IActionResult> UpdateLoanedBookAsync([FromBody] LoanBookRequest request)
        {
            logger.LogInformation("update returned date information");

            var updatedLoanedBook = await service.UpdateLoanedBookAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Update book name {Name} returned by member named {MemberName} on {ReturnedDate}",
                    updatedLoanedBook?.Book?.Name, updatedLoanedBook?.Member?.Name, DateTime.Now.Date);

            return NoContent();
        }

        [HttpPut("loan-books")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequirePermission(Permissions.UpdateLoanBooks)]
        public async Task<IActionResult> UpdateBulkLoanBooksAsync([FromBody] LoanBooksRequest request)
        {
            logger.LogInformation("update loan books information");

            await service.UpdateBulkLoanedBooksAsync(request);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Loan books updated");

            return NoContent();
        }

        #endregion Loan Books

        #region Library

        [HttpGet("count")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAllCountAsync()
        {
            try
            {
                logger.LogInformation("GetAllCountAsync");
                var members = await service.ReadMembersOnlyAsync(null, null, new PaginationRequestDto() { PageNumber = 1, PageSize = int.MaxValue });
                var books = await service.ReadBooksAsync(null, null, new PaginationRequestDto() { PageNumber = 1, PageSize = int.MaxValue });
                var loanBooks = await service.ReadLoanBooksAsync(null, null, new PaginationRequestDto() { PageNumber = 1, PageSize = int.MaxValue });
                var totalLoanedBooks = loanBooks.Count(x => x.ReturnedDate == null);

                var libraryDto = new LibraryDto(
                    NoOfMembers: members.Count(),
                    TotalNumbersOfBooks: books.Sum(b => b.BookStocks.Sum(bs => bs.Quantity)),
                    TotalLoanedBooks: totalLoanedBooks
                );

                return Ok(libraryDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while read all count");
                return BadRequest();
            }
        }

        [HttpGet("count")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAllCount2Async()
        {
            logger.LogInformation("GetAllCount2Async");

            //TODO : implement new version logic here

            return Ok(new LibraryDto(
                0,
                0,
                0
            ));
        }

        #endregion Library
    }
}