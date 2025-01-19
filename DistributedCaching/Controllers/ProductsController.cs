using DistributedCaching.Models;
using DistributedCaching.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCaching.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/v1/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAll();
            return Ok(products); // Return 200 OK with the list of products
        }

        // GET /api/v1/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if the product is not found
            }
            return Ok(product); // Return 200 OK with the product
        }

        // POST /api/v1/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreationDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 if the input is invalid
            }

            // Add the product using the service
            var createdProduct = await _productService.Add(productDto);

            // Use CreatedAtAction to return a 201 response with a location header
            return CreatedAtAction(
                nameof(GetProductById), 
                new { id = createdProduct.Id }, 
                createdProduct
            );
        }

    }
}
