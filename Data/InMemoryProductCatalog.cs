using KodaStore.Api.Models;

namespace KodaStore.Api.Data;

public sealed class InMemoryProductCatalog
{
    private readonly object _sync = new();
    private readonly List<Product> _products = [];

    public IReadOnlyList<Product> GetAll()
    {
        lock (_sync)
        {
            return _products.ToArray();
        }
    }

    public Product? GetByCode(string productCode)
    {
        lock (_sync)
        {
            return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }

    public bool TryCreateProduct(Product product)
    {
        lock (_sync)
        {
            if (_products.Any(p => string.Equals(p.ProductCode, product.ProductCode, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            _products.Add(product);
            return true;
        }
    }

    public bool TryRemove(string productCode)
    {
        lock (_sync)
        {
            var index = _products.FindIndex(p => string.Equals(p.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));

            if (index < 0) return false;

            _products.RemoveAt(index);
            return true;
        }
    }

    // public void Remove(string productCode)
    // {
    //     var product = GetByCode(productCode);
    //     if (product != null)
    //     {
    //         _products.Remove(product);
    //     }
    // }

    public bool TryUpdateProduct(Product product)
    {
        lock (_sync)
        {
            var index = _products.FindIndex(p => string.Equals(p.ProductCode, product.ProductCode, StringComparison.OrdinalIgnoreCase));

            if (index < 0) return false;

            _products[index] = product;
            return true;
        }
    }
    // public void UpdateProduct(string productCode, Product updateProduct)
    // {
    //     // Cari Product dulu
    //     var exitingProduct = GetByCode(productCode);
    //     if (exitingProduct != null)
    //     {
    //         // Cari index product lama
    //         var index = _products.IndexOf(exitingProduct);

    //         // Timpa data dengan product baru
    //         _products[index] = updateProduct;
    //     }
    // }
}