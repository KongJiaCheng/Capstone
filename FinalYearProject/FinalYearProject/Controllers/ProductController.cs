using Microsoft.AspNetCore.Mvc;
using FinalYearProject.Models;
using System.Linq;

namespace FinalYearProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Manually instantiate ECommerceDbContext
        private readonly ECommerceDbContext _context = new ECommerceDbContext();

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();  // Access the DbSet<Product>
            return Ok(products);  // Return the products in JSON format
        }
    }
}
