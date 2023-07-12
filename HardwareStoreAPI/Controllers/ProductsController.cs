using Common.Dtos;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HardwareStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductDto>>> GetProducts()
        {
            try
            {
                IList<ProductDto> productsDto = await _productService.GetAllProducts();

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                ProductDto? productDto = await _productService.GetProductById(id);
                if (productDto == null)
                {
                    return NotFound();
                }

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductDto newProduct = await _productService.CreateProduct(productDto);
                    return CreatedAtAction("GetProduct", new {id = newProduct.Id}, newProduct);
                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await _productService.UpdateProduct(productDto);

                    return NoContent();
                }
                else
                {
                    var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                    return BadRequest(validationErrors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool result = await _productService.DeleteProduct(id);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
