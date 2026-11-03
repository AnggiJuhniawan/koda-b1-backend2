using KodaStore.Api.Models;

namespace KodaStore.Api.DTOs;

public sealed record ProductResponse(string ProductCode, string Name, decimal Price, int Stock, string Category)
{
    public static ProductResponse FromModel(Product product) => new(product.ProductCode, product.Name, product.Price, product.Stock, product.Category);
}