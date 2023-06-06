using Assignment.Areas.Admin.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Areas.Admin.Controllers
{
	[Area("Admin")]
	[CheckLogin]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
