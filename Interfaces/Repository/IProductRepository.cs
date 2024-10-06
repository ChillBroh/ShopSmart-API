using MongoDB.Bson;
using webApi.Models;

namespace webApi.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task CreateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetAllApprovedProducts();
        Task<IEnumerable<Product>> GetProductsBelongsToVendor(Guid vendorId);
        Task<Product> GetProductBelongsToVendorFromProdutId(Guid productId, Guid vendorId);
        Task<bool> UpdateProduct(Guid ProductId, Product product);
        Task<bool> DeleteProduct(Guid ProductId);
        Task<List<Product>> GetProductsPendingApprovalAsync();
        Task<bool> ApproveProductByProductIdAsync(Guid ProductId);
        Task<bool> UpdateStock(Guid ProductId,int PurchasedQuantity);
        Task<Product> GetProductByProductId(Guid ProductId);

    }
}
