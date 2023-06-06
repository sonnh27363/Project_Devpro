using Microsoft.AspNetCore.Mvc;
using Assignment.Models;
//thư viện mã hóa password
using BCrypt.Net;

namespace Assignment.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		//Khai báo đối tượng thao tác csdl
		public MyDbContext db = new MyDbContext();
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult LoginPost(IFormCollection fc)
		{
			string _Email = fc["email"].ToString().Trim();
			string _Password = fc["password"].ToString().Trim();
			//mã hóa password
			//Lấy 1 bản ghi trong csdl tương ứng với email
			ItemUser record = db.Users.Where(item => item.Email == _Email).FirstOrDefault();
			if (record != null)
			{
				if (BCrypt.Net.BCrypt.Verify(_Password, record.Password))
				{
					//Tạo session email
					HttpContext.Session.SetString("email", _Email);
					//đăng nhập thành công
					return Redirect("/Admin/Home");
				}
			}
			return Redirect("Admin/Account/Login?notify=login-fail");
		}
		//Logout
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("email");
			return Redirect("/Admin/Account/Login");
		}
	}
}
