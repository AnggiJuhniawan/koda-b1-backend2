// using KodaStore.Api.Models;

// namespace KodaStore.Api.Data;

// public sealed class InMemoryProductCatalog
// {
//     private readonly List<Product> _products = [];

//     public IReadOnlyList<Product> GetAll()
//     {
//         return _products.ToArray();
//     }

//     public Product? GetByCode(string productCode)
//     {
//         return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
//     }

//     public void CreateProduct(Product product)
//     {
//         if (_products.Any(p => p.ProductCode == product.ProductCode))
//         {
//             throw new ArgumentException("Product code already exists.");
//         }
//         _products.Add(product);
//     }

//     public void Remove(string productCode)
//     {
//         var product = GetByCode(productCode);
//         if (product != null)
//         {
//             _products.Remove(product);
//         }
//     }

//     public void UpdateProduct(string productCode, Product updateProduct)
//     {
//         // Cari Product dulu
//         var exitingProduct = GetByCode(productCode);
//         if (exitingProduct != null)
//         {
//             // Cari index product lama
//             var index = _products.IndexOf(exitingProduct);

//             // Timpa data dengan product baru
//             _products[index] = updateProduct;
//         }
//     }
// }