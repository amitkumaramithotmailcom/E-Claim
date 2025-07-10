using EClaim.Application.EmailService;
using EClaim.Application.Models;
using EClaim.Application.Models.Claim;
using EClaim.Application.Models.Response;
using EClaim.Application.Models.ViewModel;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace EClaim.Application.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly string _uploadFilePath = "uploads";
        private readonly IViewRenderService _viewRenderService;

        public ClaimController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env, IEmailService emailService, IConfiguration config, IViewRenderService viewRenderService)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
            _httpClient = _httpClientFactory.CreateClient("api");
            _emailService = emailService;
            _config = config;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        [Authorize(Roles = "Claimant")]
        public async Task<IActionResult> Index()
        {
            ClaimSubmissionViewModel claimSubmissionViewModel = new ClaimSubmissionViewModel();
            return View(claimSubmissionViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Claimant")]
        public IActionResult Submit() => View();

        [HttpPost]
        [Authorize(Roles = "Claimant")]
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
                    var path = Path.Combine(_env.WebRootPath, _uploadFilePath, file.FileName);
                    using var stream = System.IO.File.Create(path);
                    await file.CopyToAsync(stream);

                    claim.Documents.Add(new ClaimDocumentModel
                    {
                        FileName = file.FileName,
                        FilePath = $"/{_uploadFilePath}/{file.FileName}"
                    });
                }

            }

            var content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Claim", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            ClaimSubmissionModel result = JsonConvert.DeserializeObject<ClaimSubmissionModel>(responseBody);

            return RedirectToAction($"Details", new { id = result.Id });
        }



        [HttpGet]
        //[Authorize(Roles = "Claimant")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetStringAsync($"api/Claim?id={id}");
            var claimRequestResponse = JsonConvert.DeserializeObject<ClaimRequestResponse>(response);

            //var userEmail = User.FindFirst("emailaddress").Value;
            //string htmlContent = await _viewRenderService.RenderToStringAsync("Views/Claim/Details.cshtml", claimRequestResponse);

            //var IsEnable = bool.Parse(_config["Smtp:IsEnable"]);
            //await _emailService.SendEmailAsync(
            //   IsEnable ? userEmail : _config["Smtp:From"],
            // "Claim Request Confirm Mail", htmlContent);

            return View(claimRequestResponse);
        }

        [HttpGet]
        //[Authorize(Roles = "Adjuster,Approver,Admin")]
        public async Task<IActionResult> ClaimDetails(DateTime fromDate, DateTime toDate)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var response = await _httpClient.GetStringAsync($"api/Claim/GetClaimDetails/{fromDate.ToString("dd-MMM-yyyy")}/{toDate.ToString("dd-MMM-yyyy")}/{userId}");

            var claimRequestResponse = JsonConvert.DeserializeObject<List<ClaimRequestResponse>>(response);
            return View(claimRequestResponse);
        }

        [HttpPost]
        public async Task<IActionResult> TakeAction(ClaimStatusUpdateViewModel model)
        {
            model.UserId = int.Parse(User.FindFirst("userId").Value);

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"api/Claim/UpdateStatus", content);

            if (response.IsSuccessStatusCode)
            {
                //var claimResponse = await _httpClient.GetStringAsync($"api/Claim?id={model.Id}");
                //var claimRequestResponse = JsonConvert.DeserializeObject<ClaimRequestResponse>(claimResponse);

                //var userEmail = User.FindFirst("Email").Value;
                //string htmlContent = await _viewRenderService.RenderToStringAsync("Views/Claim/Details.cshtml", claimRequestResponse);

                //var IsEnable = bool.Parse(_config["Smtp:IsEnable"]);
                //await _emailService.SendEmailAsync(
                //   IsEnable ? userEmail : _config["Smtp:From"],
                // $"Claim Request {model.Action}", htmlContent);



                return RedirectToAction($"ClaimDetails", new { fromDate = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy"), toDate = DateTime.Now.ToString("dd-MMM-yyyy") });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update status: {response.StatusCode} - {error}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(ClaimSearchViewModel model)
        {
            if (model.FromDate != null && model.ToDate != null && model.FromDate > model.ToDate)
            {
                ModelState.AddModelError("", "FromDate cannot be greater than ToDate");
                return View(model);
            }
            var userId = int.Parse(User.FindFirst("userId").Value);
            model.UserId= userId;

            var queryParams = new Dictionary<string, string?>
                {
                    { "UserId", model.UserId.ToString() },
                    { "Status", model.Status },
                    { "ClaimType", model.ClaimType },
                    { "FromDate", model.FromDate?.ToString("yyyy-MMM-dd") },
                    { "ToDate", model.ToDate?.ToString("yyyy-MMM-dd") },
                };

            var query = string.Join("&", queryParams
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));


            var response = await _httpClient.GetStringAsync($"api/Claim/GetClaimDetails?{query}");

            var claimRequestResponse = JsonConvert.DeserializeObject<List<ClaimRequestResponse>>(response);
            model.Results = claimRequestResponse;
            return View(model);
        }
    }
}
