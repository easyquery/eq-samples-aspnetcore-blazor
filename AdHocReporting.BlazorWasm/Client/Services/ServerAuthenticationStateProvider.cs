using System.Net.Http.Json;
using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

using EqDemo.Shared;

namespace EqDemo.Client.Services
{
    /// <summary>
    /// Takes the current user from the server, which keeps the sign-in in an ASP.NET Core Identity cookie.
    /// The client holds no tokens: it only asks the server who is signed in.
    /// </summary>
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        // The cookie scheme's name, so that the user counts as authenticated
        private const string AuthenticationType = "Identity.Application";

        private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly HttpClient _http;

        private Task<AuthenticationState>? _authState;

        public ServerAuthenticationStateProvider(HttpClient http)
        {
            _http = http;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => _authState ??= LoadAuthenticationStateAsync();

        /// <summary>Signs the user out on the server and drops the cached state.</summary>
        public async Task LogoutAsync()
        {
            await _http.PostAsync("api/account/logout", null);
            _authState = Task.FromResult(Anonymous);
            NotifyAuthenticationStateChanged(_authState);
        }

        private async Task<AuthenticationState> LoadAuthenticationStateAsync()
        {
            UserInfo? user;
            try {
                user = await _http.GetFromJsonAsync<UserInfo>("api/account/user");
            }
            catch (HttpRequestException) {
                return Anonymous;
            }

            if (user is not { IsAuthenticated: true })
                return Anonymous;

            var claims = new List<Claim> { new(ClaimTypes.Name, user.Name ?? "") };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, AuthenticationType, ClaimTypes.Name, ClaimTypes.Role);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
