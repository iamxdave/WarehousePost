using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using cw5.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _service;
        
        public WarehousesController(IWarehouseService service){
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterOrder(Order postOrder) {
            
        
            var order = postOrder;

            var orderId = await _service.GetOrderId(order.IdProduct, order.Amount);

            order.IdOrder = orderId;

            
            var productExist = await _service.DoesProductExist(order.IdProduct);
            var warehouseExist = await _service.DoesWarehouseExist(order.IdWarehouse);
            if(!productExist || !warehouseExist)
            {
                return NotFound("Product or warehouse not found");
            }


            var orderExist = await _service.DoesOrderExist(order.IdProduct, order.Amount, order.CreatedAt);

            
            if(!orderExist || orderId == 0)
            {
                return NotFound("Order does not exist");
            }

            var hasBeenFulfilled = await _service.HasBeenFullfiled(order.IdOrder);

            if(hasBeenFulfilled)
            {
                return Conflict("Order has been fulfilled");
            }
        
            var finalId = await _service.OrderRegistration(order);


            return Created("", $"Registered new order {finalId}");
        }
    }
}