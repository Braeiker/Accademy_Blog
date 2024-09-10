using Blog.Dto;
using Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        // Restituisci tutti i post
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        // Restituisci post per titolo
        [HttpGet("{title}")]
        public async Task<IActionResult> GetPostByTitle(string title)
        {
            var post = await _postService.GetPostByTitleAsync(title);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // Creare un nuovo post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto newPostDto)
        {
            if (newPostDto == null)
            {
                return BadRequest("Invalid post data.");
            }

            var result = await _postService.CreatePostAsync(newPostDto);
            if (result)
            {
                return CreatedAtAction(nameof(GetPostByTitle), new { title = newPostDto.TitlePost }, newPostDto);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the post.");
        }

        // Aggiornare un post esistente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostDto updatedPostDto)
        {
            if (updatedPostDto == null)
            {
                return BadRequest("Invalid post data.");
            }

            var result = await _postService.UpdatePostAsync(id, updatedPostDto);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Eliminare un post esistente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // Restituisci tutti i post con utenti e commenti
        [HttpGet("details")]
        public async Task<IActionResult> GetAllPostsWithDetails()
        {
            var posts = await _postService.GetAllPostsWithDetailsAsync();
            return Ok(posts);
        }

        // Restituisci post per titolo con utenti e commenti
        [HttpGet("details/{title}")]
        public async Task<IActionResult> GetPostByTitleWithDetails(string title)
        {
            var post = await _postService.GetPostByTitleWithDetailsAsync(title);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
    }
}
