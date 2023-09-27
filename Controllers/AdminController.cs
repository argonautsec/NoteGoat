using Microsoft.AspNetCore.Mvc;
using NoteGoat.Data;

namespace NoteGoat.Controllers;

public class AdminController : Controller
{
    public AdminController(NoteGoatContext context)
    {
    }
}
