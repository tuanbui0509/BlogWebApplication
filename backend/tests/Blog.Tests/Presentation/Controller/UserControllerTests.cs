using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using Blog.Domain.Identity;
using Blog.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Tests.Presentation.Controller
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            // Initialize mocks
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                Mock.Of<IRoleStore<ApplicationRole>>(),
                null, null, null, null);

            _mediatorMock = new Mock<IMediator>();

            // Initialize controller
            _controller = new UserController(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task Authenticate_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand { Email = "test", Password = "password" };
            var authResult = new AuthenticationResult { IsSuccess = true, AccessToken = "dummy-token" };

            _mediatorMock
                .Setup(m => m.Send(It.Is<LoginCommand>(c => c.Email == command.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Authenticate(command);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be(authResult);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var command = new LoginCommand { Email = "test", Password = "wrong-password" };
            var authResult = new AuthenticationResult { IsSuccess = false, Errors = { "Invalid credentials" } };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Authenticate(command);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Should().NotBeNull();
            unauthorizedResult!.Value.Should().BeEquivalentTo(authResult.Errors);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var command = new RegisterCommand { UserName = "test", Password = "password", Email = "test@example.com" };
            var registerResult = new AuthenticationResult { IsSuccess = true, AccessToken = "dummy-token" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registerResult);

            // Act
            var result = await _controller.Register(command);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be("Registration successful. Please check your email to confirm your account.");
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var command = new RegisterCommand { UserName = "test", Password = "password", Email = "test@example.com" };
            var registerResult = new AuthenticationResult { IsSuccess = false, Errors = { "User already exists" } };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registerResult);

            // Act
            var result = await _controller.Register(command);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.Value.Should().BeEquivalentTo(registerResult.Errors);
        }
    }
}