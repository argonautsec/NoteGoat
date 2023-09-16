using FileGoat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileGoat.Controllers
{
        public class HomeController : Controller
        {
                private ILogger<HomeController> _logger;
                private FileGoatContext _context;

                public HomeController(ILogger<HomeController> logger, FileGoatContext context)
                {
                        _logger = logger;
                        _context = context;
                }

                public IActionResult Index()
                {
                        _logger.LogInformation("Gone home");
                        return View();
                }
        }
}