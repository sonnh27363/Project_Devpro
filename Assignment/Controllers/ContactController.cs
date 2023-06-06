using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
