using Microsoft.AspNetCore.Mvc;
using Assignment.Models;
using Assignment.Areas.Admin.Attributes;
//Thư viện mã hóa
using BC = BCrypt.Net.BCrypt;
//Thư viện phân trang
using X.PagedList;

namespace Assignment.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Kiểm tra đăng nhập
    [CheckLogin]
    public class UsersController : Controller
    {
        //Tạo đối tượng thao tác csdl sd linq
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            //Di chuyển đến hàm Read
            return RedirectToAction("Read");
        }
        public IActionResult Read(int? p) 
        {
            //quy định số bản ghi trên 1 trang
            int pageSize = 4;
            //tính trang hiện tại
            int pageNumber = p ?? 1;
            //sd truy vấn linq
            List<ItemUser> users = db.Users.OrderByDescending(item => item.Id).ToList();
            //Gọi view có phân trang
            return View(users.ToPagedList(pageNumber,pageSize));
        }
        //update - GET
        public IActionResult Update(int? id)
        {
            ItemUser record = db.Users.Where(item => item.Id == id).FirstOrDefault();
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/UpdatePost/" + id;
            return View("FromCreateUpdate", record);
        }
        //update - POST
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            string name = fc["name"].ToString().Trim();
            //có thẻ lấy dữ liệu theo cách khác
            string email = Request.Form["email"].ToString().Trim();
            string password = Request.Form["password"].ToString().Trim();
            //lấy 1 bản ghi
            ItemUser record = db.Users.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                record.Name = name;
                record.Email = email;
                //nếu password ko rỗng thì update password
                if (!String.IsNullOrEmpty(password))
                {
                    //mã hóa password
                    password = BC.HashPassword(password);
                    record.Password = password;
                }
                //update bản ghi
                db.Users.Update(record);
                db.SaveChanges();
            }
            return RedirectToAction("Read");
        }
        //create - GET
        public IActionResult Create()
        {
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/CreatePost/";
            return View("FromCreateUpdate");
        }
        //create - POST
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string name = fc["name"].ToString().Trim();
            //có thẻ lấy dữ liệu theo cách khác
            string email = Request.Form["email"].ToString().Trim();
            string password = Request.Form["password"].ToString().Trim();
            //mã hóa password
            password = BC.HashPassword(password);
            //----
            ItemUser record = new ItemUser();
            record.Name = name;
            record.Email = email;
            record.Password = password;
            //thêm bản ghi
            db.Users.Add(record);
            db.SaveChanges();
            //---
            return RedirectToAction("Read");
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            ItemUser record = db.Users.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                db.Users.Remove(record);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Read");
        }
    }
}
