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

namespace Assignment.Controllers
{
    public class ProductsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Category(int? id, int? p)
        {
            int category_id = id ?? 0;
            //truyền biến category_id ra ngoài view thông qua ViewBag
            ViewBag.category_id = category_id;
            //Liệt kê các sản phẩm (nếu có id truyền vào thì sẽ liệt kê danh mục theo category_id, nếu ko thì liệt kê các sản phẩm)
            //quy định số bản ghi trên 1 trang
            int pageSize = 9;
            //Tính trang hiện tại
            int pageNumber = p ?? 1;
            List<ItemProduct> products = new List<ItemProduct>();
            //Nếu có id truyền vào
            if (category_id > 0)
                products = (from product in db.Products join category_product in db.CategoriesProducts on product.Id equals category_product.ProductId join category in db.Categories on category_product.CategoryId equals category.Id where category_product.CategoryId == category_id select product).ToList();
            else
                products = (from product in db.Products orderby product.Id descending select product).ToList();

            //---
            //orderby
            string orderby = !String.IsNullOrEmpty(Request.Query["orderby"]) ? Request.Query["orderby"] : "";
            switch (orderby)
            {
                case "name-asc":
                    products = products.OrderBy(item => item.Id).ToList();
                    break;
                case "name-desc":
                    products = products.OrderByDescending(item => item.Id).ToList();
                    break;
                case "price-asc":
                    products = products.OrderBy(item => item.Price).ToList();
                    break;
                case "price-desc":
                    products = products.OrderByDescending(item => item.Price).ToList();
                    break;
            }
            //--
            //Gọi view phân trang
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        //Chi tiết sản phẩm
        public IActionResult Detail(int? id)
        {
            id = id ?? 0; //để loại bỏ trường hợp id null
            ItemProduct record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            return View(record);
        }
        //Đánh số sao
        //Có thẻ lấy danh sách biến truyền từ url thông qua tham số truyền vào hàm
        public IActionResult Rating(int? id, int star)
        {

            //insert bản ghi vào table Rating
            ItemRating record = new ItemRating();
            record.ProductId = (int)id;
            record.Star = star;
            db.Rating.Add(record);
            db.SaveChanges();
            //return RedirectToAction("Detail","Products", new {id = id});
            return Redirect("/Products/Detail/" + id);
        }
    }
}
