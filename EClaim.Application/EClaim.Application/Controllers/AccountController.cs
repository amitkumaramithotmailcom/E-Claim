using EClaim.Application.EmailService;
using EClaim.Application.Models.Response;
using EClaim.Application.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EClaim.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory, IEmailService emailService, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
            _config = config;
            _httpClient = _httpClientFactory.CreateClient("api");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password not matched with confirm password");
                return View(model);
            }

            var response = await _httpClient.PostAsync("api/auth/register",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }
            else
            {
                var IsEnable = bool.Parse(_config["Smtp:IsEnable"]);
                await _emailService.SendEmailAsync(
                   IsEnable ? model.Email : _config["Smtp:From"],
                 "Confirm your email",
                $"Click <a href='{_config["AppBaseUrl"]}/Claim/{ConfirmEmail}/{model.Email}/test'>here</a> to confirm your email.");
               
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //var client = _httpClientFactory.CreateClient("api");

            var response = await _httpClient.PostAsync("api/auth/login",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponseViewModel>(json);

            // Parse JWT to Claims
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, result.FullName),
            new Claim(ClaimTypes.Email, result.Email),
            new Claim(ClaimTypes.Role, result.Role),
            new Claim("access_token", result.Token),
            new Claim("userId", result.UserId.ToString())
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var response = await _httpClient.GetStringAsync($"api/auth/ConfirmEmail/{userId}/{token}");
            var isSuccess = JsonConvert.DeserializeObject<bool>(response);

            if (isSuccess)
            {
               return BadRequest("Email confirmation failed.");
            }
            else
            {
                return RedirectToAction($"Details", new { userId = userId });
            }
            //    ? Ok("Email confirmed successfully.")
            //    : BadRequest("Invalid or expired token.");

           
        }
        [HttpGet("ConfirmEmailSuccess")]
        public async Task<IActionResult> ConfirmEmailSuccess(string userId)
        {
            return View();
        }
    }
}
