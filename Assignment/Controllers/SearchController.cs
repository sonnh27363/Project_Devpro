using Microsoft.AspNetCore.Mvc;
//su dung doi tuong thao tac IFormCollection
using Microsoft.AspNetCore.Http;
//nhin thay cac file .cs ben trong folder Models
using Assignment.Models;
//su dung entity framework
using Microsoft.EntityFrameworkCore;
//phan trang
using X.PagedList;
//nhin thay file CheckLogin.cs tron thu muc Attributes
using Assignment.Areas.Admin.Attributes;
//doi tuong thao tac file
using System.IO;
//su dung kieu List
using System.Collections.Generic;
//doi tuong ma hoa password
using BC = BCrypt.Net.BCrypt;

namespace Assignment.Controllers
{
	public class SearchController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult SearchProducts(int? p)
		{
			string key = Request.Query["key"];
			ViewBag._key = key;
			//quy định số bản ghi trên 1 trang
			int pageSize = 9;
			//Tính trang hiện tại
			int pageNumber = p ?? 1;
			//tìm các sản phẩm có name = key
			List<ItemProduct> list_record = db.Products.Where(item=>item.Name.Contains(key) || item.Content.Contains(key)).ToList();
			return View(list_record.ToPagedList(pageNumber,pageSize));
		}
		public IActionResult SearchPrice(int? p)
		{
			double fromPrice = Convert.ToDouble(Request.Query["fromPrice"]);
			double toPrice = Convert.ToDouble(Request.Query["toPrice"]);
			ViewBag._fromPrice = fromPrice;
			ViewBag._toPrice = toPrice;
			//quy định số bản ghi trên 1 trang
			int pageSize = 9;
			//Tính trang hiện tại
			int pageNumber = p ?? 1;
			//tìm các sản phẩm có name = key
			List<ItemProduct> list_record = db.Products.Where(item => item.Price >= fromPrice && item.Price <= toPrice).ToList();
			return View(list_record.ToPagedList(pageNumber, pageSize));
		}
		public IActionResult SearchTag(int? p,int? id)
		{
			int tag_id = id ?? 0;
			ViewBag._tag_id = tag_id;
			//quy định số bản ghi trên 1 trang
			int pageSize = 9;
			//Tính trang hiện tại
			int pageNumber = p ?? 1;
			//tìm các sản phẩm có name = key
			var list_record = from product in db.Products join tag_product in db.TagsProducts on product.Id equals tag_product.ProductId join tag in db.Tags on tag_product.TagId equals tag.Id where tag.Id == tag_id select product;
			return View(list_record.ToPagedList(pageNumber, pageSize));
		}
	}
}
