using System.ComponentModel.DataAnnotations;

namespace Blog.Dto
{
    public class PostDto
    {
        [Key]
        public Guid Id { get; set; }
        public string TitlePost { get; set; }
        public string? DescripcionPost { get; set; }
        public Guid UserId { get; set; }
    }
}
