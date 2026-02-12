using Asp.Versioning;
using Library.Authorization;
using Library.Dto.Identity;
using Library.Dto.Request;
using Library.Service;
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
        [ProducesResponseType(typeof(Guid), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [RequirePermission(Permissions.CreateUsers)]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request)
        {
            try
            {
                _logger.LogInformation("Register user");

                var newId = await _userService.RegisterUserAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("User registered with id: {UserId}", newId);

                return StatusCode(StatusCodes.Status201Created, newId);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    var email = request.Email.Replace(Environment.NewLine, string.Empty);
                    _logger.LogError(ex, "Unauthorized login attempt for user: {LoginId}", email);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering new user");

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [RequirePermission(Permissions.ReadUsers)]
        public async Task<IActionResult> ReadUserAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Read user");

                var result = await _userService.ReadUserAsync(id);
                var user = result.User;
                var roles = result.Roles;

                var userDto =
                    new UserDto(
                        Id: new Guid(user.Id),
                        FirstName: user.FirstName,
                        LastName: user.LastName,
                        Email: user.Email!,
                        Initials: user.Initials!,
                        EnableNotifications: user.EnableNotifications,
                        TwoFactorEnabled: user.TwoFactorEnabled,
                        UserName: user.UserName!,
                        LockoutEnabled: user.LockoutEnabled,
                        Roles: roles
                    );

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("User with id: {Id} has get user data", id);

                return Ok(userDto);
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
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [RequirePermission(Permissions.DeleteUsers)]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("deleting user");

                await _userService.DeleteUserAsync(id);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Deleted user with id {DeleteId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting new user");

                return BadRequest();
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUserAsync(LoginUserRequest request)
        {
            try
            {
                _logger.LogInformation("login user");

                var accessToken = await _userService.LoginUserAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var email = request.Email.Replace(Environment.NewLine, string.Empty);
                    _logger.LogInformation("user with email {Email} has successfully logged in", email);
                }

                return Ok(accessToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    var email = request.Email.Replace(Environment.NewLine, string.Empty);
                    _logger.LogError(ex, "Unauthorized login attempt for user: {LoginId}", email);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while login user");

                return BadRequest();
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [RequirePermission(Permissions.UpdateUsers)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Update user");

                await _userService.UpdateUserAsync(id, request);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("User with id: {Id} has updated user", id);

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                    _logger.LogError(ex, "Unauthorized attempt for user: {LoginId}", id);

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");

                return BadRequest();
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [RequirePermission(Permissions.ChangePasswordUsers)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                _logger.LogInformation("Change user password");

                await _userService.ChangePasswordAsync(request);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var email = request.Email.Replace(Environment.NewLine, string.Empty);
                    _logger.LogInformation("User with email: {Email} has changed password", email);
                }

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "password doesnt meet criterias");

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");

                return BadRequest();
            }
        }
    }
}