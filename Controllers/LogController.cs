using System.Diagnostics;
using System.Management.Automation;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteGoat.Areas.Identity.Models;
using NoteGoat.Logging;
namespace NoteGoat.Controllers;

[Authorize(Roles = Role.Admin)]
public class LogController : Controller
{
    public struct BackupFile
    {
        public string Name;
        public byte[] Content;
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Download(string[] fileTypes)
    {
        if (fileTypes.Length == 0)
        {
            return NotFound();
        }

        BackupFile backup;
        if (OperatingSystem.IsWindows())
        {
            backup = RunWindowsBackup(fileTypes);
        }
        else
        {
            backup = RunNixBackup(fileTypes);
        }
        return File(backup.Content, "application/octet-stream", backup.Name);
    }

    private BackupFile RunNixBackup(string[] fileTypes)
    {
        throw new NotImplementedException();
    }

    private BackupFile RunWindowsBackup(string[] fileTypes)
    {
        using var ps = PowerShell.Create();
        ps.AddCommand("Compress-Archive");
        foreach (var fileType in fileTypes)
        {
            ps.AddArgument(fileType);
        }

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Compress-Archive ");
        stringBuilder.Append("-Force ");
        stringBuilder.Append("-Path ");
        for (int i = 0; i < fileTypes.Length; i++)
        {
            string? fileType = fileTypes[i];
            stringBuilder.Append(fileType);
            if (i < fileTypes.Length - 1)
            {
                stringBuilder.Append(',');
            }
        }
        stringBuilder.Append(" -DestinationPath ");
        stringBuilder.Append("Archive.zip");
        _logger.LogInformation("Running {}", stringBuilder);
        Process process = new();
        process.StartInfo.FileName = "powershell";
        process.StartInfo.Arguments = stringBuilder.ToString();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();
        _logger.LogInformation(output);
        string err = process.StandardError.ReadToEnd();
        _logger.LogError(err);

        var bytes = System.IO.File.ReadAllBytes("Archive.zip");
        return new BackupFile { Name = "Archive.zip", Content = bytes };
    }
}
