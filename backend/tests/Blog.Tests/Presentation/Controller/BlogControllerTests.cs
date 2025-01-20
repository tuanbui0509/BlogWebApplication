using System.Security.Claims;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Blog.Application.Business.Posts.Queries.GetPostById;
using Blog.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Tests.Presentation.Controller
{
    public class PostsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PostsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_WhenUserNotAuthenticated_ReturnsUnauthorized()
        {
            // Arrange
            var command = new CreatePostCommand();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.Create(command);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Create_WhenUserAuthenticated_ReturnsCreatedPost()
        {
            // Arrange
            var command = new CreatePostCommand();
            var expectedId = Guid.NewGuid();
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "userId"),
            new Claim(ClaimTypes.Name, "username")
        };
            var identity = new ClaimsIdentity(claims, "test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _controller.Create(command);

            // Assert
            Assert.Equal(expectedId, result.Value);
            Assert.Equal("userId", command.UserId);
            Assert.Equal("username", command.UserName);
        }
    }
}