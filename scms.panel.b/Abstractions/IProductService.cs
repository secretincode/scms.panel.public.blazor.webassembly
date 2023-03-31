using scms.panel.b.Models.Products;

namespace scms.panel.b.Abstractions;

public interface IProductService
{
    Task<List<Product>> GetProducts();

    Task<Product> GetProduct(string productId);

    Task<Product> AddProduct(Product product);

    Task<Product> UpdateProduct(Product product);

    Task<Product> DeleteProduct(string productId);
}
