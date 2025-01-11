using System.Net;
using System.Security.Claims;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Blog.Application.Business.Posts.Queries.GetPostById;
using Blog.Domain.Identity;
using Blog.Shared.Result;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Blog.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Log.ForContext<PostsController>();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.Information("Starting get all posts");
            // caching
            var result = await _mediator.Send(new GetAllPostsQuery() { PageNumber = pageNumber, PageSize = pageSize });
            if (!result.Success)
            {
                _logger.Error("Error while get all posts", result);
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayersById(Guid id)
        {
            _logger.Information($"Starting get Post:{id}");
            var post = await _mediator.Send(new GetPostByIdQuery(id));
            if (post.Data == null)
            {
                _logger.Error($"Error while get Post:{id}", post);
                return StatusCode((int)HttpStatusCode.BadRequest, new FailureResult($"Cannot find posts by postId {id}", (int)HttpStatusCode.BadRequest));  // Return failure result
            }
            _logger.Information($"Get Post:{id} successfully");
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePostCommand command)
        {
            _logger.Information("Starting create posts");
            var user = User.Identity;

            if (user == null || !user.IsAuthenticated)
            {
                return Unauthorized();
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userNameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;

            command.UserId = userIdClaim;
            command.UserName = userNameClaim;

            return await _mediator.Send(command);
        }

        // [HttpPut("{id}")]
        // public async Task<ActionResult<Result<int>>> Update(int id, UpdatePlayerCommand command)
        // {
        //     if (id != command.Id)
        //     {
        //         return BadRequest();
        //     }

        //     return await _mediator.Send(command);
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Result<int>>> Delete(int id)
        // {
        //     return await _mediator.Send(new DeletePlayerCommand(id));
        // }
    }
}