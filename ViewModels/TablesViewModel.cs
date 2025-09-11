using RestautantMvc.DTOs.TableDTOs;

namespace RestautantMvc.ViewModels
{
    public class TablesViewModel
    {
        public List<TableResponse> Tables { get; set; } = new();
    }
}
