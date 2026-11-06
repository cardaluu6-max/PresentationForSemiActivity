using System.ComponentModel.DataAnnotations;

namespace AppDev_MidtermExam_.DTO
{
    public class UpdateProductDTO
    {

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string? ProductName { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Product price must be a positive number")]
        public decimal? ProductPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Product quantity must be a positive integer")]
        public int? ProductQuantity { get; set; }
        [Required(ErrorMessage = "Product capabilities are required")]
        [StringLength(maximumLength: 200, MinimumLength = 2, ErrorMessage = "Product capabilities must be between 2 and 200 characters")]
        public string? ProductCapabilities { get; set; }
    }
}
