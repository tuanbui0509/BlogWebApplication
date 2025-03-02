using System.Reflection;
using Blog.Application.Business.Posts.Commands.CreatePost;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMediator();
            services.AddValidators();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register FluentValidation for ASP.NET Core
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();  // This enables FluentValidation clients-side adapters if needed

            // Register validators
            services.AddValidatorsFromAssemblyContaining<CreatePostCommandValidator>();
            // Validator Request
        }
    }
}