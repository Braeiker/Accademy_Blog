using Blog.Service;
using Microsoft.EntityFrameworkCore;

namespace Blog.UserOutput
{
    public class OutputPost
    {
        private readonly AppDbContext _context;

        public OutputPost(AppDbContext context)
        {
            _context = context;
        }

        public static async Task GetAllPostAsync()
        {

            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer("Server=NTTD-FNQRK24\\SQLEXPRESS;Database=BLOG_DATABASE;User Id=sa; Password=sa;MultipleActiveResultSets=true;TrustServerCertificate=True;"))
                .AddScoped<PostService>()
                .AddScoped<CommentService>()
                .BuildServiceProvider();

            var postService = serviceProvider.GetService<PostService>();
            var commentService = serviceProvider.GetService<CommentService>();

            if (postService != null && commentService != null)
            {
                var posts = await postService.GetAllPostsAsync();
                var comments = await commentService.GetAllCommentsAsync();

                foreach (var post in posts)
                {
                    Console.WriteLine("Entrato");

                    Console.WriteLine($"Id: {post.Id}\nTitle: {post.TitlePost}\nDescription: {post.DescripcionPost}\nUserId: {post.UserId}");

                    var postComments = comments.Where(c => c.PostId == post.Id).ToList();
                    if (postComments.Any())
                    {
                        Console.WriteLine("Comments:");
                        foreach (var comment in postComments)
                        {
                            Console.WriteLine($"  Comment Id: {comment.Id}, Description: {comment.Description}, UserId: {comment.UserId}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No comments for this post.");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Failed to get the PostService or CommentService.");
            }
        }
    }
}
