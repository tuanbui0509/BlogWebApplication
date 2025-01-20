using Blog.Domain.Enums;

namespace Blog.Application.Business.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand
    {
        public string Title { get; set; }

        public string PostContents { get; set; }

        public string Slug { get; set; }
        
        public PublishState PublishState { get; set; }
    }
}