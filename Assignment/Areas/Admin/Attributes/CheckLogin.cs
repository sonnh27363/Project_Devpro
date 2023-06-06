using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment.Areas.Admin.Attributes
{
	public class CheckLogin: ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			//Lấy giá trị của session email
			string email = context.HttpContext.Session.GetString("email");
			if (string.IsNullOrEmpty(email))
			{
				context.HttpContext.Response.Redirect("/Admin/Account/Login");
			}
			base.OnActionExecuting(context);
		}
	}
}
