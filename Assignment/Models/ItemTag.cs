using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Assignment.Models
{
    [Table("Tags")]
    public class ItemTag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
