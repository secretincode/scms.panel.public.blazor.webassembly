using scms.panel.b.Abstractions;
using scms.panel.b.Models.Products;

namespace scms.panel.b.Services;

public class ProductService : IProductService
{
    public async Task<List<Product>> GetProducts()
    {
        throw new NotImplementedException();
    }

    public async Task<Product> GetProduct(string productId)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> AddProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> DeleteProduct(string productId)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
