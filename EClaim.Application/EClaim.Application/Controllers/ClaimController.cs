using EClaim.Application.Models;
using EClaim.Application.Models.Claim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EClaim.Application.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;

        public ClaimController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
            _httpClient = _httpClientFactory.CreateClient("api");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // call api/claims/user and deserialize to view model
            return View(/* list of ClaimListItemViewModel */);
        }

        [HttpGet]
        public IActionResult Submit() => View();

        [HttpPost]
        public async Task<IActionResult> Submit(ClaimSubmissionViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            var userId = int.Parse(User.FindFirst("userId").Value);

            var claim = new ClaimSubmissionModel
            {
                ClaimType = model.ClaimType,
                Description = model.Description,
                UserId = userId
            };

            if (model.Documents != null)
            {
                foreach (var file in model.Documents)
                {
                    var path = Path.Combine(_env.WebRootPath, "uploads", file.FileName);
                    using var stream = System.IO.File.Create(path);
                    await file.CopyToAsync(stream);

                    claim.Documents.Add(new ClaimDocumentModel
                    {
                        FileName = file.FileName,
                        FilePath = "/uploads/" + file.FileName
                    });
                }

            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/ClaimSubmission", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // call api/claims/{id} and return details view
            return View(/* ClaimDetailViewModel */);
        }

        [Authorize(Roles = "Adjuster,Approver")]
        public async Task<IActionResult> Review(int id)
        {
            // call api/claims/{id}/review and allow approval or adjustment
            return View(/* ClaimDetailViewModel */);
        }
    }
}
