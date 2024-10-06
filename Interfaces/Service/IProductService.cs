using webApi.Models;

namespace webApi.Interfaces.Service
{
    public interface IProductService
    {
        Task<bool> ApproveProductByProductIdAsync(Guid productId);
        Task CreateProduct(Product product);
        Task<bool> DeleteProduct(Guid productId);
        Task<IEnumerable<Product>> GetAllApprovedProducts();
        Task<IEnumerable<Product>> GetAllProducts();
        Task<List<Product>> GetProductsPendingApprovalAsync();
        Task<bool> UpdateProduct(Guid productId, Product product);
        Task<bool> UpdateStock(Guid ProductId, int PurchasedQuantity);
        Task<Product> GetProductByProductId(Guid ProductId);
    }
}
