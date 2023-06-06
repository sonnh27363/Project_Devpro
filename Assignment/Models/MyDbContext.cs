using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
namespace Assignment.Models
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var strConnectionString = builder.GetConnectionString("MyConnectionString").ToString();
            optionsBuilder.UseSqlServer(strConnectionString);
        }
        //khai báo table Users
        public DbSet<ItemUser> Users { get; set; } //<=> table Users
        public DbSet<ItemAdv> Adv { get; set; }
        public DbSet<ItemCategory> Categories { get; set; }
        public DbSet<ItemCategoryProduct> CategoriesProducts { get; set; }
        public DbSet<ItemCustomer> Customers { get; set; }
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemRating> Rating { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemTagProduct> TagsProducts { get; set; }
    }
}
