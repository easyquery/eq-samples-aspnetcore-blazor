namespace EqDemo.Shared
{
    /// <summary>
    /// The current user as the server reports it at <c>api/account/user</c>.
    /// The client builds its authentication state from it.
    /// </summary>
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }

        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
