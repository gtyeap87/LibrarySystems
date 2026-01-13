using Asp.Versioning;
using Library.Model.Request;
using Library.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UsersController(
        ILogger<UsersController> logger,
        IUserService userService
        ) : ControllerBase
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Register users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
        {
            try
            {
                _logger.LogInformation("Register user");

                var newId = await _userService.RegisterAsync(request);

                if (_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
                    _logger.LogInformation("User registered with id: {UserId}", newId);

                return StatusCode(StatusCodes.Status201Created, newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering new user");

                return BadRequest();
            }
        }

        /// <summary>
        /// Delete user
        /// Only admin role can delete other users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(DeleteUserRequest request)
        {
            try
            {
                _logger.LogInformation("deleting user");

                await _userService.DeleteAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("User with id: {LoginId} has deleted user with id {DeleteId}", request.LoginId, request.DeleteId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting new user");

                return BadRequest();
            }
        }

        /// <summary>
        /// Login users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(LoginUserRequest request)
        {
            try
            {
                _logger.LogInformation("login user");

                var accessToken = await _userService.LoginAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("user with email {Email} has successfully logged in", request.Email);

                return Ok(accessToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                    _logger.LogError(ex, "Unauthorized login attempt for user: {LoginId}", request.Email);

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while login user");

                return BadRequest();
            }
        }
    }
}