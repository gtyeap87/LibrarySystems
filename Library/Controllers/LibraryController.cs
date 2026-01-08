using Asp.Versioning;
using Library.Dto;
using Library.Model;
using Library.Model.Request;
using Library.Service;
using Microsoft.AspNetCore.Mvc;
using RestWebApi.Dto;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/library")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class LibraryController(
        ILogger<LibraryController> logger,
        ILibraryService service) : ControllerBase
    {
        private readonly ILogger<LibraryController> _logger = logger;
        private readonly ILibraryService _service = service;

        #region Book

        /// <summary>
        /// Get Books
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet("books")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooksAsync([FromQuery] Genre? genre, string? name, int pageSize, int pageNumber)
        {
            _logger.LogInformation("GetBooksAsync");
            var books = await _service.GetFullBooksAsync(genre, name, new PaginationRequest { PageSize = pageSize, PageNumber = pageNumber });
            var bookDtos = books.Select(b => new BookDto
            {
                Genre = b.Genre,
                Name = b.Name,
                AvailableQuantity = b.BookStocks.Sum(bs => bs.Quantity)
            });

            return Ok(bookDtos);
        }

        [HttpPost("book")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddbookAsync([FromBody] BookRequest request)
        {
            try
            {
                _logger.LogInformation("Add new book to library");
                if (request.Qty <= 0)
                    return BadRequest("Quantity must be greater than zero.");

                var newId = await _service.AddBookAsync(request);

                _logger.LogInformation("Added {Qty} new book name {Name} with ID: {Id}", request.Qty, request.Book.Name, newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new weather forecast");
            }

            return NoContent();
        }

        /// <summary>
        /// Added new books add range method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("books")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBooksAsync([FromBody] BooksRequest request)
        {
            try
            {
                _logger.LogInformation("Add new books to library");

                await _service.AddBooksAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    foreach (var book in request.Books)
                    {
                        _logger.LogInformation("Added new book name {Name}", book.Book.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new member");
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPatch("book/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatebookAsync(Guid id, [FromBody] BookRequest request)
        {
            try
            {
                _logger.LogInformation("update book information");
                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                if (request == null)
                    return BadRequest("no patch data.");

                var newId = await _service.UpdateBookAsync(request);

                _logger.LogInformation("Update {Qty} new book name {Name} with ID: {Id}", request.Qty, request.Book.Name, newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new weather forecast");
            }

            return NoContent();
        }

        [HttpDelete("book/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("delete book information");

                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                await _service.DeleteBookAsync(id);

                _logger.LogInformation("Deleted book with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book");
            }
            return NoContent();
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
        public async Task<IActionResult> GetMembersAsync([FromQuery] string? name, DateOnly? date, int pageSize, int pageNumber)
        {
            _logger.LogInformation("GetMembersAsync");

            var members2 = await _service.GetMembersOnlyAsync(name, date, new PaginationRequest { PageSize = pageSize, PageNumber = pageNumber });

            var membersDto = members2.Select(m => new MemberDto
            {
                Name = m.Name,
                JoinedDate = m.JoinedDate
            });
            return Ok(membersDto);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("member")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMemberAsync(MemberRequest request)
        {
            try
            {
                _logger.LogInformation("Add new member to library");
                var newId = await _service.AddMemberAsync(request);
                _logger.LogInformation("Added new member name {Name} with ID: {Id}", request.Member.Name, newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new member");
            }
            return NoContent();
        }

        /// <summary>
        /// Add multiple members add range method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("members")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMembersAsync(MembersRequest request)
        {
            try
            {
                _logger.LogInformation("Add new members to library");

                await _service.AddMembersAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    foreach (var member in request.Members)
                    {
                        _logger.LogInformation("Added new member name {Name}", member.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new member");
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPatch("member/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMemberAsync(Guid id, [FromBody] MemberRequest request)
        {
            try
            {
                _logger.LogInformation("update member information");
                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                if (request == null)
                    return BadRequest("no patch data.");

                var updatedMember = await _service.UpdateMemberAsync(request);

                _logger.LogInformation("Update member name {Name} with ID: {Id}", updatedMember.Name, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating member");
            }

            return NoContent();
        }

        [HttpDelete("member/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMemberAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("delete member information");

                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                await _service.DeleteMemberAsync(id);

                _logger.LogInformation("Deleted member with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting member");
            }
            return NoContent();
        }

        #endregion Member

        #region Loan Member

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("loan-books")]
        [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLoanBooksAsync([FromQuery] string? bookName, string? memberName, int pageSize, int pageNumber)
        {
            _logger.LogInformation("GetLoanBooksAsync");
            var loanBooks = await _service.GetLoanBooksAsync(bookName, memberName, new PaginationRequest { PageSize = pageSize, PageNumber = pageNumber });
            var loanBookDtos = loanBooks.Select(lb => new LoanBookDto
            {
                BookName = lb.Book.Name,
                MemberName = lb.Member.Name,
                LoanedDate = lb.LoanedDate,
                ReturnedDate = lb.ReturnedDate,
            });

            var loanBooksDetailsDto = new LoanBooksDetailsDto
            {
                LoanBooks = loanBookDtos,
                LoanedOutBooksQuantity = GetLoanedOutBooksQuantity(loanBooks)
            };
            return Ok(loanBooksDetailsDto);
        }

        private static int GetLoanedOutBooksQuantity(IEnumerable<LoanBook> loanBooks)
        {
            var totalQty = loanBooks.Count(lb => lb.ReturnedDate == null);
            return totalQty;
        }

        [HttpPost("loan-book")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddLoanBookAsync(LoanBookRequest request)
        {
            try
            {
                _logger.LogInformation("Add new loan book record");
                var newId = await _service.AddLoanBookAsync(request);
                _logger.LogInformation("Added new loan book record with ID: {Id}", newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new loan book");
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPatch("loan-book/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLoanedBookReturnedDateAsync(Guid id, [FromBody] LoanBookRequest request)
        {
            try
            {
                _logger.LogInformation("update returned date information");
                if (id == Guid.Empty)
                    return BadRequest("Id not valid.");

                if (request == null)
                    return BadRequest("no patch data.");

                var updatedLoanedBook = await _service.UpdateLoanedBookReturnedDateAsync(request);

                if (updatedLoanedBook == null)
                    return BadRequest("No loan book record found to update returned date.");

                _logger.LogInformation("Update book name {Name} returned by member named {MemberName} on {ReturnedDate}",
                    updatedLoanedBook?.Book?.Name, updatedLoanedBook?.Member?.Name, DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating member");
            }

            return NoContent();
        }

        #endregion Loan Member

        #region Library

        [HttpGet("count")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountAsync()
        {
            _logger.LogInformation("GetAllCountAsync");
            var members = await _service.GetMembersOnlyAsync(null, null, new PaginationRequest() { PageNumber = 1, PageSize = int.MaxValue });
            var books = await _service.GetBooksAsync(null, null, new PaginationRequest() { PageNumber = 1, PageSize = int.MaxValue });
            var loanBooks = await _service.GetLoanBooksAsync(null, null, new PaginationRequest() { PageNumber = 1, PageSize = int.MaxValue });
            var totalLoanedBooks = loanBooks.Count(x => x.ReturnedDate == null);

            var libraryDto = new LibraryDto(
                NoOfMembers: members.Count(),
                TotalNumbersOfBooks: books.Sum(b => b.BookStocks.Sum(bs => bs.Quantity)),
                TotalLoanedBooks: totalLoanedBooks
            );

            return Ok(libraryDto);
        }

        [HttpGet("count")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCount2Async()
        {
            _logger.LogInformation("GetAllCount2Async");

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