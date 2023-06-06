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
    public class ProductsController : Controller
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
            List<ItemProduct> products = db.Products.OrderByDescending(item => item.Id).ToList();
            //gọi view có phân trang
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        //update - GET
        public IActionResult Update(int? id)
        {
            //lấy một bản ghi tương ứng với id truyền vào
            ItemProduct record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Products/UpdatePost/" + id;
            return View("FormCreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            //lấy theo kiểu danh sách các checkbox
            string[] _Categories = fc["Category"];
            string[] _Tags = fc["Tag"];
            Double _Price = Convert.ToDouble(fc["Price"]);
            Double _Discount = !string.IsNullOrEmpty(fc["Discount"]) ? Convert.ToDouble(fc["Discount"]) : 0;
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            //lay ban ghi tuong ung voi id truyen vao
            ItemProduct record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            record.Name = _Name;
            record.Price = _Price;
            record.Discount = _Discount;
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
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _file_name);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _file_name;
            }
            //---
            db.Products.Update(record);
            db.SaveChanges();
            //---
            //xóa các bản ghi trong table CategoriesProducts để update lại bản ghi tương ứng với ProductId
            List<ItemCategoryProduct> categories_products = db.CategoriesProducts.Where(item => item.ProductId == id).ToList();
            foreach (var item in categories_products)
                db.CategoriesProducts.Remove(item);
            db.SaveChanges();
            foreach (string _CategoryId in _Categories)
            {
                ItemCategoryProduct category_product = new ItemCategoryProduct();
                category_product.CategoryId = Convert.ToInt32(_CategoryId);
                category_product.ProductId = id ?? 0;
                db.CategoriesProducts.Add(category_product);
                db.SaveChanges();
            }
            //---
            //---
            //xóa các bản ghi trong table TagsProducts để update lại bản ghi tương ứng với ProductId
            List<ItemTagProduct> tags_products = db.TagsProducts.Where(item => item.ProductId == id).ToList();
            foreach (var item in tags_products)
                db.TagsProducts.Remove(item);
            db.SaveChanges();
            foreach (string _TagId in _Tags)
            {
                ItemTagProduct tag_product = new ItemTagProduct();
                tag_product.TagId = Convert.ToInt32(_TagId);
                tag_product.ProductId = id ?? 0;
                db.TagsProducts.Add(tag_product);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Read", "Products", new { id });
        }
        //create - GET
        public IActionResult Create()
        {
            //tạo biến để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Products/CreatePost/";
            return View("FormCreateUpdate");
        }
        //create - POST
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            //lấy theo kiểu danh sách các checkbox
            string[] _Categories = fc["Category"];
            string[] _Tags = fc["Tag"];
            Double _Price = Convert.ToDouble(fc["Price"]);
            Double _Discount = !string.IsNullOrEmpty(fc["Discount"]) ? Convert.ToDouble(fc["Discount"]) : 0;
            int _Hot = !String.IsNullOrEmpty(fc["Hot"]) ? 1 : 0;
            string _Description = fc["Description"].ToString().Trim();
            string _Content = fc["Content"].ToString().Trim();
            //tao ban ghi
            ItemProduct record = new ItemProduct();
            record.Name = _Name;
            record.Price = _Price;
            record.Discount = _Discount;
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
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _file_name);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                //update gia tri vao cot Photo trong csdl
                record.Photo = _file_name;
            }
            //---
            db.Products.Add(record);
            db.SaveChanges();
            //---
            //lay id cua ban ghi vua moi insert
            int insertId = record.Id;
            //---
            //Tao ban ghi
            List<ItemCategoryProduct> categories_products = new List<ItemCategoryProduct>();
            foreach (string _CategoryId in _Categories)
            {
                ItemCategoryProduct category_product = new ItemCategoryProduct();
                category_product.CategoryId = Convert.ToInt32(_CategoryId);
                category_product.ProductId = insertId;
                db.CategoriesProducts.Add(category_product);
                db.SaveChanges();
            }
            //---
            //---
            //Tao ban ghi
            List<ItemTagProduct> tags_products = new List<ItemTagProduct>();
            foreach (string _TagId in _Tags)
            {
                ItemTagProduct tag_product = new ItemTagProduct();
                tag_product.TagId = Convert.ToInt32(_TagId);
                tag_product.ProductId = insertId;
                db.TagsProducts.Add(tag_product);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Read", "Products");
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            //Xóa bản ghi ở table CategoriesProducts
            List<ItemCategoryProduct> categories_products = db.CategoriesProducts.Where(item => item.ProductId == id).ToList();
            foreach (var item in categories_products)
                db.CategoriesProducts.Remove(item);
            //Xóa bản ghi ở table TagsProducts
            List<ItemTagProduct> tags_products = db.TagsProducts.Where(item => item.ProductId == id).ToList();
            foreach (var item in tags_products)
                db.TagsProducts.Remove(item);
            //Xóa bản ghi ở table Products
            ItemProduct record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                db.Products.Remove(record);
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Read");
        }
    }
}
