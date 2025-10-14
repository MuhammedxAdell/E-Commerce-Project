using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
