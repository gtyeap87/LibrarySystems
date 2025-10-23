using Azure.Core;
using Library.Dto;
using Library.Model;
using Library.Model.Request;
using Library.Service;
using Microsoft.AspNetCore.Mvc;
using RestWebApi.Dto;
using System.Diagnostics;

namespace Library.Controllers
{
    [ApiController]
    [Route("library")]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly ILibraryService _service;

        public LibraryController(
            ILogger<LibraryController> logger,
            ILibraryService service)
        {
            _logger = logger;
            _service = service;
        }

        #region Book

        /// <summary>
        ///
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet("books")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooksAsync([FromQuery] Genre? genre, string? name)
        {
            _logger.LogInformation("GetBooksAsync");
            var books = await _service.GetFullBooksAsync(genre, name);
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
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("members")]
        [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMembersAsync([FromQuery] string? name, DateOnly? date)
        {
            _logger.LogInformation("GetMembersAsync");

            var members2 = await _service.GetMembersOnlyAsync(name, date);

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
        public async Task<IActionResult> GetLoanBooksAsync([FromQuery] string? bookName, string? memberName)
        {
            _logger.LogInformation("GetLoanBooksAsync");
            var loanBooks = await _service.GetLoanBooksAsync(bookName, memberName);
            var loanBookDtos = loanBooks.Select(lb => new LoanBookDto
            {
                BookName = lb.Member.Name,
                MemberName = lb.Member.Name,
                LoanedDate = lb.LoanedDate,
                ReturnedDate = lb.ReturnedDate,
            });

            var loanBooksDetailsDto = new LoanBooksDetailsDto
            {
                LoanBooks = loanBookDtos,
                LoanBookQuantity = GetLoanBookQuantity(loanBooks, bookName ?? string.Empty, memberName ?? string.Empty)
            };
            return Ok(loanBooksDetailsDto);
        }

        private static int GetLoanBookQuantity(IEnumerable<LoanBook> loanBooks, string bookName, string memberName)
        {
            var totalQty = loanBooks.Count(lb => lb.Book.Name.Equals(bookName, StringComparison.OrdinalIgnoreCase)
                            || lb.Member.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
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

        /// <summary>
        /// Get library summary
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(IEnumerable<LibraryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountAsync()
        {
            _logger.LogInformation("GetAllCountAsync");
            var members = await _service.GetMembersOnlyAsync(null, null);
            var books = await _service.GetBooksAsync(null, null);
            var loanBooks = await _service.GetLoanBooksAsync(null, null);
            var totalLoanedBooks = loanBooks.Count(x => x.ReturnedDate == null);

            var libraryDto = new LibraryDto(
                NoOfMembers: members.Count(),
                TotalNumbersOfBooks: books.Sum(b => b.BookStocks.Sum(bs => bs.Quantity)),
                TotalLoanedBooks: totalLoanedBooks
            );

            return Ok(libraryDto);
        }

        #endregion Library
    }
}