using BlogWebApplication.Application.Features.Posts.Commands.CreatePost;
using BlogWebApplication.Application.Features.Posts.Commands.DeletePost;
using BlogWebApplication.Application.Features.Posts.Commands.UpdatePost;
using BlogWebApplication.Application.Features.Posts.Queries.GetAllPosts;
using BlogWebApplication.Application.Features.Posts.Queries.GetPostById;
using BlogWebApplication.Application.Features.Posts.Queries.GetPostsWithPagination;
using BlogWebApplication.Shared.Implements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApplication.WebApi.Controllers
{
    public class PostsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<GetAllPostsDto>>>> Get()
        {
            return await _mediator.Send(new GetAllPostsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetPostByIdDto>>> GetPostsById(Guid id)
        {
            return await _mediator.Send(new GetPostByIdQuery(id)); 
        }

        // [HttpGet]
        // [Route("club/{clubId}")]
        // public async Task<ActionResult<Result<List<GetPostsByClubDto>>>> GetPostsByClub(int clubId)
        // {
        //     return await _mediator.Send(new GetPostsByClubQuery(clubId));
        // }

        [HttpGet]
        [Route("paged")]
        public async Task<ActionResult<PaginatedResult<GetPostsWithPaginationDto>>> GetPostsWithPagination([FromQuery] GetPostsWithPaginationQuery query)
        {
            var validator = new GetPostsWithPaginationValidator();

            // Call Validate or ValidateAsync and pass the object which needs to be validated
            var result = validator.Validate(query);

            if (result.IsValid)
            {
                return await _mediator.Send(query);
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages); 
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> Create(CreatePostCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id, UpdatePostCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            return await _mediator.Send(command); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<Guid>>> Delete(Guid id)
        {
            return await _mediator.Send(new DeletePostCommand(id)); 
        }
    }
}