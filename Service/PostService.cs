using Blog.Dto;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service
{
    public class PostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> Save()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while saving changes.", ex);
            }
        }

        // Restituisci tutti i post
        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    TitlePost = p.TitlePost,
                    DescripcionPost = p.DescripcionPost,
                    UserId = p.UserId
                })
                .ToListAsync();
        }

        // Restituisci post per titolo
        public async Task<PostDto> GetPostByTitleAsync(string title)
        {
            return await _context.Posts
                .Where(p => p.TitlePost == title)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    TitlePost = p.TitlePost,
                    DescripcionPost = p.DescripcionPost,
                    UserId = p.UserId
                })
                .FirstOrDefaultAsync();
        }

        // Creare un nuovo post
        public async Task<bool> CreatePostAsync(PostDto newPostDto)
        {
            if (newPostDto == null)
            {
                throw new ArgumentNullException(nameof(newPostDto));
            }

            // Verifica se l'utente esiste
            var userExists = await _context.Users.AnyAsync(u => u.Id == newPostDto.UserId);
            if (!userExists)
            {
                // Gestisci il caso in cui l'utente non esiste
                throw new ArgumentException("L'utente specificato non esiste.");
            }

            var newPost = new Post
            {
                Id = newPostDto.Id,
                TitlePost = newPostDto.TitlePost,
                DescripcionPost = newPostDto.DescripcionPost,
                UserId = newPostDto.UserId
            };

            _context.Posts.Add(newPost);
            return await Save();
        }


        // Aggiornare un post esistente
        public async Task<bool> UpdatePostAsync(Guid id, PostDto updatedPostDto)
        {
            if (updatedPostDto == null)
            {
                throw new ArgumentNullException(nameof(updatedPostDto));
            }

            var existingPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.TitlePost = updatedPostDto.TitlePost;
            existingPost.DescripcionPost = updatedPostDto.DescripcionPost;
            existingPost.UserId = updatedPostDto.UserId;

            _context.Posts.Update(existingPost);
            return await Save();
        }

        // Eliminare un post esistente
        public async Task<bool> DeletePostAsync(Guid id)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post == null)
                {
                    return false;
                }

                _context.Posts.Remove(post);
                return await Save();
            }
            catch (Exception ex)
            {
                // Log the exception details here (e.g., using a logging framework)
                throw new ApplicationException("An error occurred while deleting the post.", ex);
            }
        }

        // Restituisci tutti i post con utenti e commenti
        public async Task<List<Post>> GetAllPostsWithDetailsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ToListAsync();
        }

        // Restituisci post per titolo con utenti e commenti
        public async Task<Post> GetPostByTitleWithDetailsAsync(string title)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.TitlePost == title);
        }

        // Metodo non implementato
        internal async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
