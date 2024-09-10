using Blog.Dto;
using Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        // Restituisci tutti i commenti
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        // Restituisci commento per ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // Creare un nuovo commento
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto newCommentDto)
        {
            if (newCommentDto == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var result = await _commentService.CreateCommentAsync(newCommentDto);
            if (result)
            {
                return CreatedAtAction(nameof(GetCommentById), new { id = newCommentDto.Id }, newCommentDto);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the comment.");
        }

        // Aggiornare un commento esistente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentDto updatedCommentDto)
        {
            if (updatedCommentDto == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var result = await _commentService.UpdateCommentAsync(id, updatedCommentDto);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Eliminare un commento esistente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Restituisci tutti i commenti con utenti e post
        [HttpGet("details")]
        public async Task<IActionResult> GetAllCommentsWithDetails()
        {
            var comments = await _commentService.GetAllCommentsWithDetailsAsync();
            return Ok(comments);
        }

        // Restituisci commento per ID con dettagli utente e post
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetCommentByIdWithDetails(Guid id)
        {
            var comment = await _commentService.GetCommentByIdWithDetailsAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }
}
