﻿using MongoDB.Bson;
using webApi.Models;
using webApi.Models.Enums;

namespace webApi.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderByOrderId(Guid OrderId);
        Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId);
        Task CreateOrder(Order order);
        Task<bool> UpdateOrderStatus(Guid OrderId, string newStatus);
        Task<bool> DeleteOrder(Guid OrderId);
    }
}
