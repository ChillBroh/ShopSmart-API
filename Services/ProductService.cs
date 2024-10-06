using MongoDB.Driver;
using webApi.Interfaces.Repository;
using webApi.Models;
using webApi.Interfaces.Service;
using webApi.Repositories;

namespace webApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> ApproveProductByProductIdAsync(Guid ProductId)
        {
          
            return await _productRepository.ApproveProductByProductIdAsync(ProductId);
        }
        public async Task CreateProduct(Product product)
        {
            await _productRepository.CreateProduct(product);
        }
        public async Task<bool> DeleteProduct(Guid ProductId)
        {
            return await _productRepository.DeleteProduct(ProductId);
        }
        public async Task<IEnumerable<Product>> GetAllApprovedProducts()
        {
            return await _productRepository.GetAllApprovedProducts();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public Task<Product> GetProductByProductId(Guid ProductId)
        {
            return _productRepository.GetProductByProductId(ProductId);
        }

        public async Task<List<Product>> GetProductsPendingApprovalAsync()
        {
            return await _productRepository.GetProductsPendingApprovalAsync();
        }

        public async Task<bool> UpdateProduct(Guid ProductId, Product product)
        {
            return await _productRepository.UpdateProduct(ProductId, product);
        }

        public async Task<bool> UpdateStock(Guid ProductId, int PurchasedQuantity)
        {
            return await _productRepository.UpdateStock(ProductId, PurchasedQuantity);
        }
    }
}
