using RestautantMvc.DTOs.MenuDTOs;

namespace RestautantMvc.ViewModels
{
    public class MenuViewModel
    {
        public List<MenuItemResponse> MenuItems { get; set; } = new ();
    }
}
