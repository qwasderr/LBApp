using Microsoft.AspNetCore.Mvc;

namespace LBApp.Controllers
{
    public class DiagramsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
