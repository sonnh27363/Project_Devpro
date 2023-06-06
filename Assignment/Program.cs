var builder = WebApplication.CreateBuilder(args);
//--
//Đăng ký sử dụng mô hình MVC
builder.Services.AddControllersWithViews();
//--
//Đăng ký sử dụng session
builder.Services.AddSession();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//--
//sử dụng session
app.UseSession();
//Sử dụng url
app.UseStaticFiles(); // Muốn truy cập được các file trong thư mục wwwroot thì phải có dòng nà
app.UseRouting();
app.MapControllerRoute(
			name: "area",
			pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
//--
app.Run();
