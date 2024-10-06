using MongoDB.Driver;
using webApi.Models.Enums;
using webApi.Models;
using webApi.Data;
using webApi.Interfaces.Repository;

namespace webApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoDbContext context)
        {
            _orders = context.Orders;
        }


        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orders.Find(order => true).ToListAsync();  
        }

        public async Task<Order> GetOrderByOrderId(Guid orderId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            return await _orders.Find(filter).FirstOrDefaultAsync();  
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.UserId, userId);
            return await _orders.Find(filter).ToListAsync();  
        }


        public async Task CreateOrder(Order order)
        {
            if (order.OrderId == Guid.Empty)
            {
                order.OrderId = Guid.NewGuid();  
            }
            order.CreatedAt = DateTime.UtcNow;
            await _orders.InsertOneAsync(order);  
        }

        
        public async Task<bool> UpdateOrderStatus(Guid orderId, string newStatus)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            var update = Builders<Order>.Update
                          .Set(o => o.OrderStatus, newStatus)
                          .Set(o => o.UpdateAt, DateTime.UtcNow); 

            var result = await _orders.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0; 
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            var result = await _orders.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
