using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Service
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal principal = _anonymous;
            try
            {
                if (_jsRuntime is IJSInProcessRuntime)
                {
                    var userEmail = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "userEmail");
                    var adminUsername = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "adminUsername");

                    var claims = new List<Claim>();

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        claims.Add(new Claim(ClaimTypes.Name, userEmail));
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    if (!string.IsNullOrEmpty(adminUsername))
                    {
                        claims.Add(new Claim(ClaimTypes.Name, adminUsername));
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    if (claims.Any())
                        principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
                }
            }
            catch
            {
            }

            return new AuthenticationState(principal);
        }


        public async Task NotifyLoginAsync(string identifier, string role)
        {
            if (role == "User")
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "userEmail", identifier);
            }
            else if (role == "Admin")
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "adminUsername", identifier);
            }

            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task NotifyLogoutAsync(string role)
        {
            if (role == "User")
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "userEmail");
            }
            else if (role == "Admin")
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "adminUsername");
            }

            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        private ClaimsPrincipal CreateUserPrincipal(string identifier, string role)
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, identifier),
            new Claim(ClaimTypes.Role, role)
        }, "apiauth");

            return new ClaimsPrincipal(identity);
        }

        public async Task RefreshStateAsync()
        {
            var userEmail = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "userEmail");
            var adminUsername = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "adminUsername");

            ClaimsPrincipal principal = _anonymous;

            if (!string.IsNullOrEmpty(userEmail))
                principal = CreateUserPrincipal(userEmail, "User");
            else if (!string.IsNullOrEmpty(adminUsername))
                principal = CreateUserPrincipal(adminUsername, "Admin");

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }
    }
}

