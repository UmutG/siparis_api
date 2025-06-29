using Microsoft.AspNetCore.Mvc;

namespace SiparisAPI.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private static Order[] Orders = new[]
        {
           new Order { OrderId = 1, Date = DateTime.Now, ItemId = 1, Unit = 2 },
           new Order { OrderId = 2, Date = DateTime.Now.AddDays(-1), ItemId = 2, Unit = 5 },
           new Order { OrderId = 3, Date = DateTime.Now.AddDays(-2), ItemId = 3, Unit = 3 },
        };

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetOrder")]
        public IEnumerable<Order> Get(int id)
        {
            return Orders.Where(o => o.OrderId == id);
        }

        [HttpGet("all", Name = "GetAllOrders")]
        public IEnumerable<Order> GetAll()
        {
            return Orders;
        }

        [HttpPost(Name = "CreateOrder")]
        public IActionResult Post(Order order)
        {
            if (order is not Order)
            {
                _logger.LogError("Order is null");
                return BadRequest("Order cannot be null");
            }

            if (order.ItemId <= 0 || order.Unit <= 0)
            {
                _logger.LogError("Invalid order data: {Order}", order);
                return BadRequest("Invalid order data");
            }

            var inStock = new ProductController().CheckStock(order.ItemId, order.Unit);

            if (!inStock)
            {
                _logger.LogWarning("Insufficient stock for ItemId: {ItemId}, Unit: {Unit}", order.ItemId, order.Unit);
                return BadRequest("Insufficient stock");
            }
            order.OrderId = Orders.Length > 0 ? Orders.Max(o => o.OrderId) + 1 : 1;
            Orders = Orders.Append(order).ToArray();

            _logger.LogInformation("Order created successfully: {OrderId}", order.OrderId);
            return CreatedAtAction(nameof(Get), new { id = order.OrderId }, order);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public IActionResult Delete(int id)
        {
            var order = Orders.FirstOrDefault(o => o.OrderId == id);
            if (order is not Order)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                return NotFound("Order not found");
            }
            Orders = Orders.Where(o => o.OrderId != id).ToArray();
            _logger.LogInformation("Order deleted successfully: {OrderId}", id);
            return NoContent();
        }
    }
}
