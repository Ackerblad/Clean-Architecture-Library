using Application.Commands.Books.CreateBook;
using Application.Commands.Books.DeleteBook;
using Application.Commands.Books.UpdateBook;
using Application.Queries.Books.GetAllBooks;
using Application.Queries.Books.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var result = await _mediator.Send(new GetAllBooksQuery());

                _logger.LogInformation("Books retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving books.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id));

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to retrieve book with ID {Id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Book with ID {Id} retrieved successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving book with ID {Id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to create book: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Book created successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to update book with ID {id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Book with ID {id} updated successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteBookCommand(id));

                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Failed to delete book with ID {Id}: {Message}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Book with ID {Id} deleted successfully.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting book with ID {Id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
