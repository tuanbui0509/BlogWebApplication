using FluentValidation;

namespace BlogWebApplication.Application.Features.Posts.Queries.GetPostsWithPagination
{
    public class GetPostsWithPaginationValidator: AbstractValidator<GetPostsWithPaginationQuery>
    {
        public GetPostsWithPaginationValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}