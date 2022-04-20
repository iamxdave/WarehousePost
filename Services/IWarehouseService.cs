using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.Services
{
    public interface IWarehouseService
    {
        public Task<int> OrderRegistration(Order order); 
        public Task<bool> DoesProductExist(int productId);
        public Task<bool> DoesWarehouseExist(int warehouseId);
        public Task<bool> DoesOrderExist(int productId, int amount, DateTime createdDate);
        public Task<bool> HasBeenFullfiled(int orderId);
        public Task<int> GetOrderId(int productId, int amount);
    }
}