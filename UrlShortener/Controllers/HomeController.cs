using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers {
    public class HomeController : Controller {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db) { db = _db; }
        public IActionResult Index(UrlManager obj, int val=0) { return View(obj); }

        // POST : Create URL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UrlManager obj) {
            
            if (string.IsNullOrEmpty(obj.Url) || !Uri.TryCreate(obj.Url, UriKind.Absolute, out var inputUrl))
                return View(obj);

            const string chars = "ABCEDFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890@$&";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, 8)
                .Select(x => x[random.Next(x.Length)])
                .ToArray()
            );

            while(db.Urls.Any(u => u.ShortUrl == randomString)) {
                randomString = new string(Enumerable.Repeat(chars, 8)
                    .Select(x => x[random.Next(x.Length)])
                    .ToArray()
                );
            }
            var urlObj = new UrlManager() {
                Url = obj.Url,
                ShortUrl = randomString
            };

            db.Urls.Add(urlObj);
            db.SaveChanges();

            var ctx = HttpContext;

            urlObj = new UrlManager() {
                Url = obj.Url,
                ShortUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host.Host}:{ctx.Request.Host.Port}/SUrl/{randomString}"
                /*ShortUrl = $"localhost:44313/SUrl/{randomString}"*/
            };
            return RedirectToAction("Index", urlObj);
        }

        public IActionResult RedirectToOriginalUrl(string shortURL) {
            var urlObj = db.Urls.FirstOrDefault(u => u.ShortUrl == shortURL);
            if (urlObj == null) { return NotFound(); }

            return Redirect(urlObj.Url);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
