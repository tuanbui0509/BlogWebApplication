using FluentValidation;

namespace Blog.Application.Business.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.PostContents)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(20).WithMessage("Content must be at least 20 characters long.");
            // RuleFor(x => x.PublishedDate)
            //     .LessThanOrEqualTo(DateTime.Now).WithMessage("Published date cannot be in the future.");
            RuleFor(x => x.Tags)
            .NotNull().WithMessage("Tags are required.")
            .Must(tags => tags.Count <= 10).WithMessage("You can specify up to 10 tags.");
        }
    }
}