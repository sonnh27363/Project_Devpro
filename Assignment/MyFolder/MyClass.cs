using Assignment.Models;

namespace Assignment.MyFolder
{
	public class MyClass
	{
		public static MyDbContext db = new MyDbContext();
		//Lấy danh sách ảnh quảng cáo theo vị trí
		public static List<ItemAdv> GetAdv(int _position)
		{
			List<ItemAdv> list_record = db.Adv.Where(item => item.Position == _position).OrderByDescending(item => item.Id).ToList();
			return list_record;
		}
	}
}
