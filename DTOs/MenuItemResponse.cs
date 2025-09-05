using RestautantMvc.Models;

namespace RestautantMvc.DTOs
{
    public class MenuItemResponse
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsPopular { get; set; }
        public string? BildUrl { get; set; }
        public MenuCategory Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
