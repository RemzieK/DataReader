using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    public class ConsoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
