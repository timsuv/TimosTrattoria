using System.ComponentModel.DataAnnotations;

namespace RestautantMvc.DTOs
{
    public class CreateTableRequest
    {
        [Required]
        [Display(Name = "Table Number")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }
    }
}
