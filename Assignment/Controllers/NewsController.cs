using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Assignment.Controllers
{
	public class NewsController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult Index()
		{
            return View();
        }
	}
}
