using RestautantMvc.DTOs;

namespace RestautantMvc.ViewModels
{
    public class MenuViewModel
    {
        public List<MenuItemResponse> MenuItems { get; set; } = new ();
    }
}
