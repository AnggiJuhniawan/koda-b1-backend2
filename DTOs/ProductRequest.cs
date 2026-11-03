using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public record ProductRequest(
    [Required, StringLength(30)]
    string ProductCode,

    [Required]
    [StringLength(120)]
    string Name,

    [Required]
    [Range(typeof(decimal), "0", "99999999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    decimal Price,

    [Required]
    [Range(0, int.MaxValue)]
    int Stock,

    [Required, StringLength(120)]
    string Category
);