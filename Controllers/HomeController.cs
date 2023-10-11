using NoteGoat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

namespace NoteGoat.Controllers;

public class HomeController : Controller
{
        private ILogger<HomeController> _logger;
        private NoteGoatContext _context;

        public HomeController(
                ILogger<HomeController> logger,
                NoteGoatContext context
        )
        {
                _logger = logger;
                _context = context;
        }

        public IActionResult Index()
        {
                ViewBag.RedirectUrl = string.Format(
                        "{0}://{1}{2}/Home/CheatSheet",
                        Request.Scheme,
                        Request.Host,
                        Request.PathBase
                );
                return View();
        }

        public IActionResult About(string redirectUrl)
        {
                return Redirect(redirectUrl);
        }

        public IActionResult CheatSheet()
        {
                return View();
        }
}
