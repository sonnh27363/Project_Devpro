using Microsoft.AspNetCore.Mvc;
//nhìn thấy các file trong thư mục Models
using Assignment.Models;
//nhìn thấy các file trong thư mục Attributes
using Assignment.Areas.Admin.Attributes;
//thư viện phân trang
using X.PagedList;
using Microsoft.EntityFrameworkCore;
//thao tác với file
using System.IO;

namespace Assignment.Areas.Admin.Controllers
{
    [Area("Admin")]
    //kiểm tra đăng nhập
    [CheckLogin]
    public class NewsController : Controller
    {
        //tạo đối tượng thao tác csdl sử dụng linq
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            //di chuyển đến hàm Read
            return RedirectToAction("Read");
        }
        public IActionResult Read(int? p)
        {
            //quy định số bản ghi trên một trang
            int pageSize = 50;
            //tính trang hiện tại
            int pageNumber = p ?? 1;
            List<ItemNews> News = db.News.OrderByDescending(item => item.Id).ToList();
            //gọi view có phân trang
            return View(News.ToPagedList(pageNumber, pageSize));
        }
        //update - GET
        public IActionResult Update(int? id)
        {
            //lấy một bản ghi tương ứng với id truyền vào
            ItemNews record = db.News.Where(item => item.Id == id).FirstOrDefault();
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/News/UpdatePost/" + id;
            return View("FormCreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            //lay ban ghi tuong ung voi id truyen vao
            ItemNews record = db.News.Where(item => item.Id == id).FirstOrDefault();
            record.Name = _Name;
            record.Hot = _Hot;
            record.Description = _Description;
            record.Content = _Content;
            //---
            //lay thong tin cua file
            string _file_name = "";
            try
            {
                //lay ten file
                _file_name = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_file_name))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();
                _file_name = timestamp + "_" + _file_name;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _file_name);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _file_name;
            }
            //---
            db.News.Update(record);
            db.SaveChanges();            
            //---
            return RedirectToAction("Read", "News", new { id });
        }
        //create - GET
        public IActionResult Create()
        {
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/News/CreatePost/";
            return View("FormCreateUpdate");
        }
        //create - POST
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            //tao ban ghi
            ItemNews record = new ItemNews();
            record.Name = _Name;
            record.Hot = _Hot;
            record.Description = _Description;
            record.Content = _Content;
            //---
            //lay thong tin cua file
            string _file_name = "";
            try
            {
                //lay ten file
                _file_name = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_file_name))
            {
                //upload anh moi
                var timestamp = DateTime.Now.ToFileTime();
                _file_name = timestamp + "_" + _file_name;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _file_name);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _file_name;
            }
            //---
            db.News.Add(record);
            db.SaveChanges();
            //---            
            return RedirectToAction("Read", "News");
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            //xóa bản ghi ở table News
            ItemNews record = db.News.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                db.News.Remove(record);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Read");
        }
    }
}
