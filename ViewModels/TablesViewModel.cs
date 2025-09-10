using RestautantMvc.DTOs;

namespace RestautantMvc.ViewModels
{
    public class TablesViewModel
    {
        public List<TableResponse> Tables { get; set; } = new();
    }
}
