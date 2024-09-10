using System.ComponentModel.DataAnnotations;

namespace Blog.Dto
{
    public class UserDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }


    }
}
