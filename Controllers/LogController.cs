using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteGoat.Areas.Identity.Models;
using NoteGoat.Logging;
namespace NoteGoat.Controllers;

[Authorize(Roles = Role.Admin)]
public class LogController : Controller
{
    private readonly ILogger<LogController> _logger;
    private readonly string _logFilePath;

    public LogController(ILogger<LogController> logger)
    {
        _logger = logger;
        _logFilePath = FileLoggerExtensions.GetNoteGoatLogFilePath();
    }

    public IActionResult Index()
    {
        // TODO: Get last n lines instead
        ViewBag.Logs = System.IO.File.ReadAllText(_logFilePath, Encoding.UTF8);
        return View();
    }
}
