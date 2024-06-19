using Blog.Application.Common.Interfaces;
using Blog.Domain.Common;
using Blog.Domain.Common.Interfaces;
using Blog.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<ITokenService, TokenService>();
        }
    }
}