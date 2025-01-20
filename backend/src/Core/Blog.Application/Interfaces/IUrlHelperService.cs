namespace Blog.Application.Interfaces
{
    public interface IUrlHelperService
    {
        string GenerateCallbackUrl(string action, string controller, object routeValues);
    }
}