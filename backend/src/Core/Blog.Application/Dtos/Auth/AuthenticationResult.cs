namespace Blog.Application.Dtos.Auth
{
    public class AuthenticationResult
    {
        // Indicates if the authentication attempt was successful
        public bool IsSuccess { get; set; }

        // JWT or access token issued to the user
        public string AccessToken { get; set; }

        // Refresh token for refreshing session without re-authenticating
        public string RefreshToken { get; set; }

        // Unique identifier of the authenticated user
        public string UserId { get; set; }

        // Username of the authenticated user
        public string Username { get; set; }

        // Roles assigned to the user
        public List<string> Roles { get; set; } = new List<string>();

        // List of errors if the authentication failed
        public List<string> Errors { get; set; } = new List<string>();

        // Expiration date and time of the access token
        public DateTime ExpiresAt { get; set; }

        // Helper property to check if there were any errors
        public bool HasErrors => Errors?.Count > 0;
    }
}