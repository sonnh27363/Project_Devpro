using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
	public class IntroduceController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
