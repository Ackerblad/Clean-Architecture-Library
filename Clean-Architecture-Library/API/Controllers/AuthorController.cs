using Application.Commands.Authors.CreateAuthor;
using Application.Commands.Authors.DeleteAuthor;
using Application.Commands.Authors.UpdateAuthor;
using Application.Queries.Authors.GetAllAuthors;
using Application.Queries.Authors.GetAuthorById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var result = await _mediator.Send(new GetAllAuthorsQuery());

                _logger.LogInformation("Authors retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving authors.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetAuthorByIdQuery(id));

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to retrieve author with ID {Id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Author with ID {Id} retrieved successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving author with ID {Id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to create author: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Author created successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating an author.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to update author with ID {id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Author with ID {id} updated successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating an Author.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAuthorCommand(id));

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to delete author with ID {Id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Author with ID {Id} deleted successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting author with ID {Id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
