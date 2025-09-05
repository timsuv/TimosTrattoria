using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestautantMvc.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

      
        public string? Name { get; set; }
      
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsPopular { get; set; } = false;
        public string? BildUrl { get; set; }
        public MenuCategory Category { get; set; }
    }
}
