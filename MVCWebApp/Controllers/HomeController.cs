using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MVCWebApp.Areas.Services;
using MVCWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections;


namespace MVCWebApp.Controllers
{
    public class UserInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class ReverserClass : IComparer
    {
        // Call CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(object x, object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(y, x));
        }
    }


    public class HomeController : Controller
    {

        //some dependency injection happening here.....
        private readonly ILogger<HomeController> _logger;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMailService _mailService;
        private readonly ICyberSecurity _cyberSecurity;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [TempData]
        public string StatusMessage { get; set; }

        public HomeController(
            ICyberSecurity cybersecurity,
            IMailService mailService,
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment
            )//, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mailService = mailService;
            _cyberSecurity = cybersecurity;
            _hostingEnvironment = hostingEnvironment;
            //_userManager = userManager;
            //_httpContextAccessor = httpContextAccessor;

        }


        public IActionResult SendMail()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] MailRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StatusMessage = await _mailService.SendEmailAsync(request);
                    StatusMessage = StatusMessage.Contains("Requested mail action okay, completed") ? "Success! Email sent" : "Email not sent";
                    return RedirectToAction("Index");
                    //return Ok();
                }
                catch (Exception ex)
                {
                    StatusMessage = "Error sending email.";// ex.Message;
                    return View("SendMail");
                    //throw;
                }

            }
            StatusMessage = "Error sending email.";
            return View("SendMail");

        }
        public string KeepAlive()
        {
            return "heartbeat";
        }

        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == null)
            {
                return Content("File Name is Empty...");
            }

            // get the filePath
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "ServerFiles", fileName);

            // create a memorystream
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            // set the position to return the file from
            memoryStream.Position = 0;

            // Get the MIMEType for the File
            var mimeType = (string file) =>
            {
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(file, out string contentType);
                //var extension = Path.GetExtension(file).ToLowerInvariant();
                return contentType;
            };

            return File(memoryStream, mimeType(filePath), Path.GetFileName(filePath));
        }


        [Authorize(Roles = "SuperAdmin")]
        public IActionResult FileIndex()
        {

            //var fileModels = new FilesModels();

            var provider = new PhysicalFileProvider(_hostingEnvironment.ContentRootPath);
            var contents = provider.GetDirectoryContents("wwwroot/downloads");// string.Empty);

            //var filePath = Path.Combine("wwwroot", "js", "site.js");
            //var fileInfo = provider.GetFileInfo(filePath);

            //var list = contents.ToArray();
            //Array.Sort(list, (f1, f2) => f2.IsDirectory.CompareTo(f1.IsDirectory));

            var result = contents.OrderBy(p => !p.IsDirectory).ThenBy(p => p.Name);

            return View(result);
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CookieSet([FromBody] CookiePreferences cookieprefs)
        {
            if (cookieprefs != null)
            {
                SetCookie("siteCookie", JsonSerializer.Serialize(cookieprefs), null);
            }
            return Json(cookieprefs);
        }
        public IActionResult Pivot()
        {
            return View();
        }
        public IActionResult Pivot2()
        {
            //throw new NotImplementedException();
            return View();
        }
        public IActionResult Pivot3()
        {
            return View(GetUser());
        }
        public IActionResult Graphic()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Test()
        {
            RemoveCookie("treeKey");
            TestViewModel model = new TestViewModel()
            { TestString = "Enter your word(s) with spaces" };

            Func<int> getRandomNumber = () => new Random().Next(1, 100);
            model.RandomNumber = getRandomNumber();

            //model.TestString = new(model.TestString.ToCharArray().Where(i => i != 32).ToArray());
            return View(model);
        }

        [HttpPost]
        public IActionResult Test(TestViewModel model)
        {

            model.TestString = new(model.TestString.ToCharArray().Where(i => i != 32).ToArray());
            return View(model);
        }

        bool CheckNNHSNumber(string pNHSNumber)
        {
            return pNHSNumber.ToCharArray().Where(i => i >= 48 && i <= 57).Count() == 10 && new List<int>() {
                    pNHSNumber.ToCharArray()
                    .Where<char>((value, index) => index < 9)
                    .Select<char, int>((value, index) => (10 - index) * (value - 48))
                    .Sum()
                }
                .Select<int, int>(i => i % 11)
                .Select<int, int>(i => (11 - i) == 11 ? 0 : (11 - i))
                .First() == (pNHSNumber[pNHSNumber.Length - 1] - 48);
        }

        [Authorize]
        public IActionResult Privacy()
        {

            return View(GetUser());
        }
        private UserInfo GetUser()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.FindFirstValue(ClaimTypes.Name);// will give the user's userName

            // For ASP.NET Core <= 3.1
            //ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            //string userEmail = applicationUser?.Email; // will give the user's Email

            // For ASP.NET Core >= 5.0
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's Email

            return new UserInfo()
            {
                Name = userName,
                ID = userId,
                Email = userEmail

            };

        }

        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.MaxValue;//DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns>  
        public string Get(string key)
        {
            return Request.Cookies[key];
        }
        public IActionResult JSTree()
        {
            //EITHER

            //read cookie from IHttpContextAccessor  
            //string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["treeKey"];

            //OR

            //read cookie from Request object  
            string cookieValueFromReq = Request.Cookies["treeKey"];

            cookieValueFromReq = string.IsNullOrEmpty(cookieValueFromReq) ? "No cookie was found" : cookieValueFromReq;

            //Delete the cookie object  
            RemoveCookie("treeKey");

            //Automatically populate JS Tree with some NODES
            List<string> urls = new List<string>() { cookieValueFromReq };
            int level = 4;
            IList<TreeNode> treeNodes = new List<TreeNode>();
            for (int i = 1; i <= 12; i++)
            {
                string strParent = (i - 1) % level == 0 ? "#" : $"nd{((i - 1) / level * level) + 1}";  //don't cancel the level- its a DIV operation
                string thisIcon = strParent == "#" ? "fa fa-folder" : "fa fa-file";
                var node = new TreeNode()
                {
                    id = $"nd{i}",
                    parent = strParent,
                    text = $"Node {i}",
                    data = i == 6 ? urls.FirstOrDefault() : "",
                    icon = thisIcon

                };
                treeNodes.Add(node);
            }

            //set the key value in Cookie  
            SetCookie("treeKey", "This is the cookie stored on your computer", 10);

            return View(treeNodes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
