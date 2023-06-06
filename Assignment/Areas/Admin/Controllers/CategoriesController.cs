using Assignment.Areas.Admin.Attributes;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using X.PagedList;

namespace Assignment.Areas.Admin.Controllers
{
	[Area("Admin")]
	[CheckLogin]
	public class CategoriesController : Controller
	{
		//Tạo biến để lưu chuỗi kết nối
		public string strConnectionString = "";
        //hàm tạo
        public CategoriesController()
        {
			//Đọc thông tin từ file appsettings.json để lưu chuỗi kết nối vào biến strConnectionString
			var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			//Lấy chuỗi kết nối từ file appsettings.json
			strConnectionString = config.GetConnectionString("MyConnectionString").ToString();
        }
        public IActionResult Index()
		{
			return RedirectToAction("Read");
		}
		public IActionResult Read(int? page)
		{
			int pageSize = 20;
			int pageNumber = page ?? 1;
			//--
			//Tạo đối tượng DataTable
			DataTable dtCategories = new DataTable();
			using (SqlConnection conn = new SqlConnection(strConnectionString))
			{
				SqlDataAdapter da = new SqlDataAdapter("select * from Categories where ParentId = 0 order by Id desc", conn);
				//Đổ dữ liệu vào biến DataTable
				da.Fill(dtCategories);
			}
			//Tạo biến kkieeur List để convert DataTable sang kiểu List
			List<ItemCategory> listCategories = new List<ItemCategory>();
			foreach (DataRow item in dtCategories.Rows)
			{
				ItemCategory record = new ItemCategory();
				record.Id = Convert.ToInt32(item["id"]);
				record.ParentId = Convert.ToInt32(item["ParentId"]);
				record.Name = item["Name"].ToString();
				//---
				listCategories.Add(record);
			}
			//--
			return View(listCategories.ToPagedList(pageNumber,pageSize));
		}
		//update
		public IActionResult Update(int? id)
		{
			//--
			//Tao doi tuong DataTable
			DataTable dtCategories = new DataTable();
			using (SqlConnection conn = new SqlConnection(strConnectionString))
			{
				SqlDataAdapter da = new SqlDataAdapter("select * from Categories where Id=" + id, conn);
				//do du lieu vao bien DataTable
				da.Fill(dtCategories);
			}
			//khởi tạo một item (tương ứng với 1 row trang table) của DataTable dtCategories
			DataRow itemCategory = dtCategories.NewRow();
			if (dtCategories.Rows.Count > 0)
				itemCategory = dtCategories.Rows[0];
			//tạo biến để đưa vào thuộc tính action của thẻ form
			ViewBag.action = "/Admin/Categories/UpdatePost/" + id;
			return View("FormCreateUpdate", itemCategory);
		}
		//update - POST
		[HttpPost]
		public IActionResult updatePost(int? id, IFormCollection fc)
		{
			string _Name = fc["Name"].ToString().Trim();
			int _ParentId = Convert.ToInt32(fc["ParentId"].ToString());
			int _DisplayHomePage = fc["DisplayHomePage"] != "" && fc["DisplayHomePage"] == "on" ? 1 : 0;
			using (SqlConnection conn = new SqlConnection(strConnectionString))
			{
				//phai open ket noi khi thuc hien udpate, insert
				conn.Open();
				SqlCommand cmd = new SqlCommand("update Categories set Name = @var_name, ParentId = @var_parent_id, DisplayHomePage = @var_display_home_page where Id = @var_id", conn);
				cmd.Parameters.AddWithValue("@var_name", _Name);
				cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
				cmd.Parameters.AddWithValue("@var_display_home_page", _DisplayHomePage);
				cmd.Parameters.AddWithValue("@var_id", id);
				//thuc thi cau lenh sql
				cmd.ExecuteNonQuery();
			}
			return RedirectToAction("Read");
		}
		//create - GET
		public IActionResult Create()
		{
			//tạo biến để đưa vào thuộc tính action của thẻ form
			ViewBag.action = "/Admin/Categories/CreatePost";
			return View("FormCreateUpdate");
		}
		//create - POST
		[HttpPost]
		public IActionResult CreatePost(IFormCollection fc)
		{
			string _Name = fc["Name"].ToString().Trim();
			int _ParentId = Convert.ToInt32(fc["ParentId"].ToString());
			int _DisplayHomePage = fc["DisplayHomePage"] != "" && fc["DisplayHomePage"] == "on" ? 1 : 0;
			//return Content(_ParentId.ToString());
			using (SqlConnection conn = new SqlConnection(strConnectionString))
			{
				//phai open ket noi khi thuc hien udpate, insert
				conn.Open();
				SqlCommand cmd = new SqlCommand("insert into Categories(Name,ParentId,DisplayHomePage) values(@var_name, @var_parent_id,@var_display_home_page)", conn);
				cmd.Parameters.AddWithValue("@var_name", _Name);
				cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
				cmd.Parameters.AddWithValue("@var_display_home_page", _DisplayHomePage);
				//thuc thi cau lenh sql
				cmd.ExecuteNonQuery();
			}
			//---
			return RedirectToAction("Read");
		}
		public IActionResult Delete(int? id)
		{
			using (SqlConnection conn = new SqlConnection(strConnectionString))
			{
				//phai open ket noi khi thuc hien udpate, insert
				conn.Open();
				SqlCommand cmd = new SqlCommand("delete from Categories where Id=@var_id or ParentId=@var_id", conn);
				cmd.Parameters.AddWithValue("@var_id", id);
				//thuc thi cau lenh sql
				cmd.ExecuteNonQuery();
			}
			return RedirectToAction("Read");
		}
	}
}
