//using DOOH.Server.Models;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace DOOH.Adboard.Services
//{
//    public partial class SecurityService
//    {

//        private readonly IConfiguration _configuration;
//        private readonly HttpClient _httpClient;

//        public ApplicationUser User { get; private set; } = new ApplicationUser { Name = "Anonymous" };

//        public ClaimsPrincipal Principal { get; private set; }

//        public SecurityService(IConfiguration configuration, HttpClient httpClient)
//        {
//            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//        }

//        public bool IsInRole(params string[] roles)
//        {
//#if DEBUG
//            if (User.Name == "admin")
//            {
//                return true;
//            }
//#endif

//            if (roles.Contains("Everybody"))
//            {
//                return true;
//            }

//            if (!IsAuthenticated())
//            {
//                return false;
//            }

//            if (roles.Contains("Authenticated"))
//            {
//                return true;
//            }

//            return roles.Any(role => Principal.IsInRole(role));
//        }

//        public bool IsAuthenticated()
//        {
//            return Principal?.Identity.IsAuthenticated == true;
//        }

//        public async Task<bool> InitializeAsync(AuthenticationState result)
//        {
//            Principal = result.User;
//            var userId = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (userId != null && User?.Id != userId)
//            {
//                var response = await _httpClient.GetAsync($"odata/Identity/ApplicationUsers('{userId}')?$expand=Roles");
//                if (response != null)
//                {
//                    var content = await response.Content.ReadAsStringAsync();
//                    User = JsonSerializer.Deserialize<ApplicationUser>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ApplicationUser { Name = "Anonymous" };
//                }
//                else
//                {
//                    User = new ApplicationUser { Name = "Anonymous" };
//                }
//            }

//            return IsAuthenticated();
//        }


//        public async Task<ApplicationAuthenticationState> GetAuthenticationStateAsync()
//        {
//            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/Account/CurrentUser"));

//            //return await response.ReadAsync<ApplicationAuthenticationState>(); // Avoid using Radzen code in this project of the book
//            if (response != null)
//            {
//                var content = await response.Content.ReadAsStringAsync();
//                var result = JsonSerializer.Deserialize<ApplicationAuthenticationState>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//                return result;
//            }

//            return new ApplicationAuthenticationState { IsAuthenticated = false };
//        }

//        public async Task Logout()
//        {
//            await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/Account/Logout"));
//            User = new ApplicationUser { Name = "Anonymous" };
//            Principal = new ClaimsPrincipal(new ClaimsIdentity());
//        }

//        public async Task Login()
//        {

//            var userName = _configuration.GetValue<string>("Service:UserName");
//            var password = _configuration.GetValue<string>("Service:Password");

//            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
//            {
//                return;
//            }

//            var formContent = new FormUrlEncodedContent(new[]
//            {
//                new KeyValuePair<string, string>("userName", userName),
//                new KeyValuePair<string, string>("password", password),
//                new KeyValuePair<string, string>("redirectUrl", "/")
//            });
//            var response = await _httpClient.PostAsync("/Account/Login", formContent);
//            if (response != null)
//            {
//                var cookie = response.Headers.GetValues("Set-Cookie").First();
//                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
//                await Console.Out.WriteLineAsync(cookie);

//            }
//            else
//            {
//                User = new ApplicationUser { Name = "Anonymous" };
//            }
//        }


//        /*

//        public async Task<string> Login(string baseUrl, string userName, string password)
//        {

//            //var userName = _configuration.GetValue<string>("Service:UserName");
//            //var password = _configuration.GetValue<string>("Service:Password");

//            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
//            {
//                throw new ArgumentNullException("Error: UserName or Password");
//            }

//            var formContent = new FormUrlEncodedContent(new[]
//            {
//                new KeyValuePair<string, string>("userName", userName),
//                new KeyValuePair<string, string>("password", password),
//                new KeyValuePair<string, string>("redirectUrl", "/")
//            });
//            var response = await _httpClient.PostAsync($"{baseUrl}/Account/Login", formContent);
//            if (response != null)
//            {
//                return response.Headers.GetValues("Set-Cookie").First();
//            }

//            throw new HttpRequestException("Error: Login failed");
//        }*/

//    }
//}
