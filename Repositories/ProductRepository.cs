using MongoDB.Bson;
using MongoDB.Driver;
using webApi.Data;
using webApi.Interfaces.Repository;
using webApi.Models;

namespace webApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbContext context)
        {
            _products = context.Products;
        }

        public async Task<bool> ApproveProductByProductIdAsync(Guid ProductId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, ProductId);
            var update = Builders<Product>.Update.Set(p => p.AdminApproved, true);

            var result = await _products.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(Guid ProductId)
        {
            var result = await _products.DeleteOneAsync(product => product.ProductId == ProductId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        

        public async Task<IEnumerable<Product>> GetAllApprovedProducts()
        {
            var filter = Builders<Product>.Filter.Eq(p => p.AdminApproved, true);

            return await _products.Find(filter).ToListAsync();
        }

        public async  Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

  
        public async Task<List<Product>> GetProductsPendingApprovalAsync()
        {
            var filter = Builders<Product>.Filter.Eq(p => p.AdminApproved, false);

            return await _products.Find(filter).ToListAsync();
        }

       

        public async Task<bool> UpdateProduct(Guid ProductId, Product product)
        {
            var result = await _products.ReplaceOneAsync(p => p.ProductId == ProductId, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<Product?> GetProductBelongsToVendorFromProdutId(Guid productId, Guid vendorId )
        {
            
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(p =>p.ProductId, productId),
                Builders<Product>.Filter.Eq(p => p.VendorId, vendorId)
            );

            var product = await _products.Find(filter).FirstOrDefaultAsync();

            if (product == null)
            {
                return null; 
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsBelongsToVendor(Guid vendorId)
        {
       
            var filter = Builders<Product>.Filter.Eq(p => p.VendorId, vendorId);

            var products = await _products.Find(filter).ToListAsync();

            return products;
        }

        public async Task<bool> UpdateStock(Guid ProductId, int PurchasedQuantity)
        {
            
            var filter = Builders<Product>.Filter.And(
                    Builders<Product>.Filter.Eq(p => p.ProductId, ProductId),
                    Builders<Product>.Filter.Eq(p => p.AdminApproved, true)
            );
            var product = await _products.Find(filter).FirstOrDefaultAsync();
            if (product != null) { 
                if(product.Stock > PurchasedQuantity)
                {
                    var update = Builders<Product>.Update
                                    .Set(p => p.Stock, product.Stock-PurchasedQuantity)
                                    .Set(p => p.UpdateAt, DateTime.UtcNow);
                    var result = await _products.UpdateOneAsync(filter, update);
                    return result.ModifiedCount > 0;
                }
                else
                {
                    return false;
                }
               
            }
            else
            {
                return false;
            }
            
        }

        public async Task<Product> GetProductByProductId(Guid ProductId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, ProductId);
            var x = await _products.Find(filter).FirstOrDefaultAsync();
            return x;
        }
    }
}
