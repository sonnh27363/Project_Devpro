using Microsoft.AspNetCore.Mvc;
//nhin thay cac file .cs ben trong folder Models
using Assignment.Models;

namespace Assignment.Controllers
{
    public class CartController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            //Lấy giỏ hàng
            List<Item> cart = Cart.GetCart(HttpContext.Session);
            if(cart != null)
            {
                ViewBag.cart = cart;
            }
            return View();
        }
        //Thêm sản phẩm vào giỏ hàng
        public IActionResult Buy(int id)
        {
            //gọi hàm CartAdd từ Cart trong thư mục Models để thêm sản phẩm vào session cart
            Cart.CartAdd(HttpContext.Session, id);
            return RedirectToAction("Index");
        }
        //Xóa sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            Cart.CartRemove(HttpContext.Session, id);
            return RedirectToAction("Index");
        }
        //Xóa toàn bộ sản phẩm khỏi giỏ hàng
        public IActionResult Destroy()
        {
			Cart.CartDestroy(HttpContext.Session);
			return RedirectToAction("Index");
		}
		//Cập nhật số lượng sản phẩm trong giỏ hàng
		public IActionResult Update()
		{
			List<Item> cart = Cart.GetCart(HttpContext.Session);
			foreach (var item in cart)
			{
				int quantity = Convert.ToInt32(Request.Form["product_" + item.ProductRecord.Id]);
				Cart.CartUpdate(HttpContext.Session, item.ProductRecord.Id, quantity);
			}
			return RedirectToAction("Index");
		}
        //Thanh toán đơn hàng
        public IActionResult CheckOut()
        {
            //kiểm tra người dùng đã đăng nhập chưa
            string customer_id = HttpContext.Session.GetString("customer_id");
            if (!String.IsNullOrEmpty(customer_id))
            {
                Cart.CartCheckOut(HttpContext.Session, Convert.ToInt32(customer_id));
            }
            else
                return Redirect("/Account/Login");
            return Redirect("/Home");
        }
	}
}
