using Blog.Application.Abstractions;
using Blog.Application.Common.Interfaces;
using Blog.Application.Helpers;
using Blog.Application.Interfaces;
using Blog.Application.Services;
using Blog.Domain.Common;
using Blog.Domain.Common.Interfaces;
using Blog.Infrastructure.Authentication;
using Blog.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.ConfigureJwt(configuration);
            services.AddRedisCache(configuration);
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddTransient<IUrlHelperService, UrlHelperService>();
        }

        // Add redis cache
        private static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
        }

        private static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            // Adding Authentication
            services.AddAuthentication(options =>
            {
                // options.DefaultAuthenticateScheme =
                // options.DefaultChallengeScheme =
                // options.DefaultForbidScheme =
                // options.DefaultSignOutScheme =
                // options.DefaultSignInScheme =
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // For APIs by default
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;  // Browser login challenge defaults to Cookies
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)  // Cookie scheme for interactive login
            // .AddFacebook(options =>
            // {
            //     options.AppId = configuration["Authentication:Facebook:AppId"];
            //     options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            //     options.CallbackPath = configuration["Authentication:Facebook:CallbackPath"];
            // })
            // .AddOAuth("GitHub", options =>
            // {
            //     options.ClientId = "GITHUB_CLIENT_ID";
            //     options.ClientSecret = "GITHUB_CLIENT_SECRET";
            //     options.CallbackPath = new PathString("/auth/github/callback");
            //     options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            //     options.TokenEndpoint = "https://github.com/login/oauth/access_token";
            //     options.SaveTokens = true;
            // });

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JwtSettings:SigningKey"])
                    ),
                    ValidateLifetime = true
                };
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = configuration["Authentication:Google:CallbackPath"];
            });

            // Set email confirmation lifetime for registration
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(1); // Set token expiration time to 5 minutes
            });
        }
    }
}