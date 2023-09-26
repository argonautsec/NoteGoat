using NoteGoat.Data;
using Microsoft.AspNetCore.Mvc;

namespace NoteGoat.Controllers;

public class HomeController : Controller
{
        private ILogger<HomeController> _logger;
        private NoteGoatContext _context;


        public HomeController(ILogger<HomeController> logger, NoteGoatContext context)
        {
                _logger = logger;
                _context = context;
        }

        public IActionResult Index()
        {
                return View();
        }
}