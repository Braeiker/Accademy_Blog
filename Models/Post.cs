using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string TitlePost { get; set; }
        public string? DescripcionPost { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
