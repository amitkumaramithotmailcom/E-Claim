using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EClaim.Application.EmailService;
using EClaim.Application.Models;
using EClaim.Application.Models.Response;
using EClaim.Application.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace EClaim.Application.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public UsersController(IHttpClientFactory httpClientFactory, IEmailService emailService, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
            _config = config;
            _httpClient = _httpClientFactory.CreateClient("api");
        }

        // GET: UsersController
        public async Task<IActionResult> Index(UsersSearchViewModel model)
        {
            if (model.FromDate != null && model.ToDate != null && model.FromDate > model.ToDate)
            {
                ModelState.AddModelError("", "FromDate cannot be greater than ToDate");
                return View(model);
            }

            var userId = int.Parse(User.FindFirst("userId").Value);
            model.UserId = userId;

            bool? IsEmailVerified = null;
            if (!string.IsNullOrEmpty(model.IsEmailVerified))
            {
                IsEmailVerified = model.IsEmailVerified.Equals("Yes") ? true : false;
            }

            var queryParams = new Dictionary<string, string?>
                {
                    { "UserId", model.UserId.ToString() },
                    { "FullName", model.FullName },
                    { "Email", model.Email },
                    { "Phone", model.Phone },
                    { "Role", model.Role },
                    { "IsEmailVerified", IsEmailVerified.ToString() },
                    { "FromDate", model.FromDate?.ToString("yyyy-MMM-dd") },
                    { "ToDate", model.ToDate?.ToString("yyyy-MMM-dd") },
                };

            var query = string.Join("&", queryParams
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));


            var response = await _httpClient.GetStringAsync($"api/Users/GetUsers?{query}");
            var usersViewModel = JsonConvert.DeserializeObject<List<UsersViewModel>>(response);

            model.Results = usersViewModel;
            return View(model);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetStringAsync($"api/Users/{id}");
            var usersViewModel = JsonConvert.DeserializeObject<UsersViewModel>(response);
            return View(usersViewModel);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsersViewModel model)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var user = new UserCreateModel
            {
                Id = id,
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Role = model.Role,
                IsEmailVerified = model.IsEmailVerified,
                UserId = userId
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Users", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            ClaimSubmissionModel result = JsonConvert.DeserializeObject<ClaimSubmissionModel>(responseBody);
            return RedirectToAction(nameof(Index));

        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
