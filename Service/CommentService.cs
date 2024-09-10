using Blog.Dto;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service
{
    public class CommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> Save()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Restituisci tutti i commenti
        public async Task<List<CommentDto>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    UserId = c.UserId,
                    PostId = c.PostId
                })
                .ToListAsync();
        }

        // Restituisci commento per ID
        public async Task<CommentDto> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments
                .Where(c => c.Id == id)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    UserId = c.UserId,
                    PostId = c.PostId
                })
                .FirstOrDefaultAsync();
        }

        // Creare un nuovo commento
        public async Task<bool> CreateCommentAsync(CommentDto newCommentDto)
        {
            if (newCommentDto == null)
            {
                throw new ArgumentNullException(nameof(newCommentDto));
            }

            var newComment = new Comment
            {
                Id = newCommentDto.Id,
                Description = newCommentDto.Description,
                UserId = newCommentDto.UserId,
                PostId = newCommentDto.PostId
            };

            _context.Comments.Add(newComment);
            return await Save();
        }

        // Aggiornare un commento esistente
        public async Task<bool> UpdateCommentAsync(Guid id, CommentDto updatedCommentDto)
        {
            if (updatedCommentDto == null)
            {
                throw new ArgumentNullException(nameof(updatedCommentDto));
            }

            var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (existingComment == null)
            {
                return false;
            }

            existingComment.Description = updatedCommentDto.Description;
            existingComment.UserId = updatedCommentDto.UserId;
            existingComment.PostId = updatedCommentDto.PostId;

            _context.Comments.Update(existingComment);
            return await Save();
        }

        // Eliminare un commento esistente
        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
                if (comment == null)
                {
                    return false;
                }

                _context.Comments.Remove(comment);
                return await Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Restituisci tutti i commenti con utenti e post
        public async Task<List<Comment>> GetAllCommentsWithDetailsAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .ToListAsync();
        }

        // Restituisci commento per ID con dettagli utente e post
        public async Task<Comment> GetCommentByIdWithDetailsAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
