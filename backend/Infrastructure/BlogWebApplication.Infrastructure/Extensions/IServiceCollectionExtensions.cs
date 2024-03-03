using BlogWebApplication.Application.Interfaces;
using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Common.Interfaces;
using BlogWebApplication.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlogWebApplication.Infrastructure.Extensions
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
                .AddTransient<IEmailService, EmailService>();
        }
    }
}