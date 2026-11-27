using AppDev_MidtermExam_.Common;
using AppDev_MidtermExam_.DTO;
using AppDev_MidtermExam_.Models;
using AppDev_MidtermExam_.Properties.Data;
using AppDev_MidtermExam_.Repositories;
using AppDev_MidtermExam_.Repositories.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace AppDev_MidtermExam_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductControllers : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductControllers(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Student")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var productDTO = products.Adapt<List<ProductResponseDTO>>();

            return Ok(new ApiResponse<List<ProductResponseDTO>>
            {
                Success = true,
                Message = "Products retrieved successfully.",
                Data = productDTO.ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Product with ID:{id} is not found",
                    Data = null
                });
            }

            var productDTO = product.Adapt<ProductResponseDTO>();
            return Ok(new ApiResponse<ProductResponseDTO>
            {
                Success = true,
                Message = "Products retrieved successfully.",
                Data = productDTO
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productModel = product.Adapt<Products>();


            var rows = await _productRepository.CreateAsync(productModel);

            return rows > 0 ? Ok("Product Added") : BadRequest("Product was not added");

        }

        //[Authorize(Roles = "Admin")]
        //[HttpPut("{id}")]
        //public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDTO product)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    using var connection = _db.CreateConnection();

        //    var rows = connection.Execute(
        //        "UPDATE Products SET ProductName = @ProductName, ProductPrice = @ProductPrice, ProductQuantity = @ProductQuantity, ProductCapabilities = @ProductCapabilities WHERE Id = @Id",
        //        new { Id = id, ProductName = product.ProductName, ProductPrice = product.ProductPrice, ProductQuantity = product.ProductQuantity, ProductCapabilities = product.ProductCapabilities }
        //        );

        //    return rows > 0 ? Ok("Product Updated") : BadRequest("Product was not updated");
        //}
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{id}")]
        //public IActionResult DeleteProduct(int id)
        //{
        //    using var connection = _db.CreateConnection();

        //    var rows = connection.Execute(
        //        "DELETE FROM Products WHERE Id = @Id",
        //        new { Id = id }
        //        );

        //    return rows > 0 ? Ok("Product Deleted") : BadRequest("Product was not deleted");
        //}
    }
}
