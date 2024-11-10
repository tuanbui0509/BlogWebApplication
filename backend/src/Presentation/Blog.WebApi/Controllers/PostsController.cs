using System.Security.Claims;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Blog.Shared;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = nameof(Roles.Admin) + "," + nameof(Roles.SuperAdmin) + "," + nameof(Roles.User))]
    public class PostsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;


        public PostsController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllPostsQuery());
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result);  // Return failure result
            }
            return Ok(result);  // Return success result
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Result<GetPostByIdDto>>> GetPlayersById(int id)
        // {
        //     return await _mediator.Send(new GetPlayerByIdQuery(id));
        // }

        // [HttpGet]
        // [Route("club/{clubId}")]
        // public async Task<ActionResult<Result<List<GetPlayersByClubDto>>>> GetPlayersByClub(int clubId)
        // {
        //     return await _mediator.Send(new GetPlayersByClubQuery(clubId));
        // }

        // [HttpGet]
        // [Route("paged")]
        // public async Task<ActionResult<PaginatedResult<GetPlayersWithPaginationDto>>> GetPlayersWithPagination([FromQuery] GetPlayersWithPaginationQuery query)
        // {
        //     var validator = new GetPlayersWithPaginationValidator();

        //     // Call Validate or ValidateAsync and pass the object which needs to be validated
        //     var result = validator.Validate(query);

        //     if (result.IsValid)
        //     {
        //         return await _mediator.Send(query);
        //     }

        //     var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        //     return BadRequest(errorMessages);
        // }

        // [HttpPost]
        // public async Task<ActionResult<Guid>> Create(CreatePostCommand command)
        // {
        //     var user = User.Identity;

        //     if (user == null || !user.IsAuthenticated)
        //     {
        //         return Unauthorized();
        //     }
        //     var claimsIdentity = User.Identity as ClaimsIdentity;
        //     var userIdClaim = claimsIdentity?.FindFirst("Id")?.Value;
        //     var userNameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;

        //     command.UserId = userIdClaim;
        //     command.UserName = userNameClaim;

        //     return await _mediator.Send(command);
        // }

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