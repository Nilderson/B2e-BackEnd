using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Produtos.Core.Entities;
using Produtos.Core.Services;
using Produtos.Infra.Data;

namespace _2.Produtos.Products.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {

        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.ListAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Product product)
        {
            var success = await _productService.AddProduct(product);
            if (!success) return BadRequest("Update não foi realizado.");
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest("product ID URL e Objeto a ser editado, não Coincidem.");
            var success = await _productService.UpdateProduct(product);
            if (!success) return BadRequest("Update não foi realizado.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteProduct(id);
            if (!success) return NotFound();
            return NoContent();
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            return await _productService.ExportProductsToExcel();
        }
    }
}
