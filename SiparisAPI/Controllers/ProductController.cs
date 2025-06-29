using Microsoft.AspNetCore.Mvc;

namespace SiparisAPI.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : Controller
    {
        private static Product[] Products = new[]
        {
            new Product { ProductId = 1, Name = "Laptop", Description = "High performance laptop", Price = 1500.00, Stock = 10 },
            new Product { ProductId = 2, Name = "Mouse", Description = "Wireless mouse", Price = 25.00, Stock = 50 },
            new Product { ProductId = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 75.00, Stock = 30 },
        };

        [HttpGet(Name = "GetProduct")]
        public IEnumerable<Product> Get(int id)
        {
            return Products.Where(p => p.ProductId == id);
        }

        [HttpGet("all", Name = "GetAllProducts")]
        public IEnumerable<Product> GetAll()
        {
            return Products;
        }

        [HttpPost(Name = "CreateProduct")]
        public IActionResult Post(Product product)
        {
            Products = Products.Append(product).ToArray();
            return CreatedAtAction(nameof(Get), new { id = product.ProductId }, product);
        }

        [HttpGet("check-stock", Name = "CheckStock")]
        public bool CheckStock(int productId, int quantity)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return false; // Product not found
            }
            if (product.Stock >= quantity)
            {
                product.Stock -= quantity;
                return true;
            }
            return false;
        }
    }
}
