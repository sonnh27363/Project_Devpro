using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    [Table("Slides")]
    public class ItemSile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
