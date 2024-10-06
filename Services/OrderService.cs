using webApi.Interfaces.Repository;
using webApi.Interfaces.Service;
using webApi.Models;
using webApi.Models.Enums;

namespace webApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrder(Order order)
        {
             await _orderRepository.CreateOrder(order);
        }

        public async Task<bool> DeleteOrder(Guid OrderId)
        {
            return await _orderRepository.DeleteOrder(OrderId);  
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async  Task<Order> GetOrderByOrderId(Guid OrderId)
        {
            return await _orderRepository.GetOrderByOrderId(OrderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId)
        {
            return await _orderRepository.GetOrdersByUserId(userId);
        }

        public async Task<bool> UpdateOrderStatus(Guid OrderId, string newStatus)
        {
            return await _orderRepository.UpdateOrderStatus(OrderId, newStatus);
        }
    }
}
