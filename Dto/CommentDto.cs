using System.ComponentModel.DataAnnotations;

namespace Blog.Dto
{
    public class CommentDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
