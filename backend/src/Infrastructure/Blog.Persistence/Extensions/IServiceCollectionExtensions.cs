using Blog.Application.Abstractions;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Persistence.Data.Contexts;
using Blog.Persistence.Data.Seeds;
using Blog.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMappings();
            services.AddDbContext(configuration);
            // Register ApplicationDbContextFactory for DI
            services.AddSingleton<ApplicationDbContextFactory>();
            services.AddRepositories();
            services.AddSeedData();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient<IPostRepository, PostRepository>()
                .AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        }
        private static void AddSeedData(this IServiceCollection services)
        {
            services
                .AddScoped<SeedData>();
        }
    }
}