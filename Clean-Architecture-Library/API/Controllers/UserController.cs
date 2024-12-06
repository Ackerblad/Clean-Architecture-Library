using Application.Commands.Users.CreateUser;
using Application.Queries.Users.GetAllUsers;
using Application.Queries.Users.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersQuery());

                _logger.LogInformation("Users retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving users.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to register user: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("User registered successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during registration.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to login: {Message}", result.Message);
                    return Unauthorized(result);
                }

                _logger.LogInformation("Login successful.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
