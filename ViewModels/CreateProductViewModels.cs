using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.ViewModels;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Productcode is Required")]
    public string ProductCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is Required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is Required")]
    [Range(0, 9999999999999.99, ErrorMessage = "Price cannot be negative")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Stock is Required")]
    public int? Stock { get; set; }

    [Required(ErrorMessage = "Category is Required")]
    public string Category { get; set; } = string.Empty;
}