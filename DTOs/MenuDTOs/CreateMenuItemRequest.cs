using RestautantMvc.Models;
using System.ComponentModel.DataAnnotations;

namespace RestautantMvc.DTOs.MenuDTOs
{
    public class CreateMenuItemRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsPopular { get; set; }

        [Url]
        public string? BildUrl { get; set; }

        [Required]
        public MenuCategory Category { get; set; }
    }
}
