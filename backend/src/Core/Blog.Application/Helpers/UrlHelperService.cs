using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Blog.Application.Helpers
{
    public class UrlHelperService : IUrlHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateCallbackUrl(string action, string controller, object routeValues)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available.");

            var request = httpContext.Request;

            var host = $"{request.Scheme}://{request.Host}";
            var pathBase = request.PathBase.HasValue ? request.PathBase.Value : string.Empty;

            var route = new RouteValueDictionary(routeValues);

            var queryParameters = route
            .ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty)
            .AsEnumerable();

            var actionUrl = $"{host}{pathBase}/{controller}/{action}";
            var queryString = QueryString.Create(queryParameters);

            return $"{actionUrl}{queryString}";
        }
    }
}