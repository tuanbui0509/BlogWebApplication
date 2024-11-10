using FluentValidation;

namespace Blog.Application.Business.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(500)
                .NotEmpty();
            RuleFor(v => v.PostContents)
                .NotEmpty();
        }
    }
}