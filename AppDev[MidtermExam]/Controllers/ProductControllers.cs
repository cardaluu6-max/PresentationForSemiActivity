using AppDev_MidtermExam_.DTO;
using AppDev_MidtermExam_.Properties.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AppDev_MidtermExam_.Common;

namespace AppDev_MidtermExam_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductControllers : ControllerBase
    {
        public readonly DatabaseHelper _db;

        public ProductControllers(DatabaseHelper db)
        {
            _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllProducts()
        {

            using var connection = _db.CreateConnection();

            var products = connection.Query<ProductResponseDTO>(
                "SELECT ProductName, ProductPrice, ProductQuantity, ProductCapabilities FROM Products"
                );
            return Ok(new ApiResponse<List<ProductResponseDTO>> { 
                Success = true, 
                Message = "Products retrieved successfully.", 
                Data = products.ToList() 
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id) 
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            using var connection = _db.CreateConnection();

            var products = connection.Query<ProductResponseDTO>(
                "SELECT ProductName, ProductPrice, ProductQuantity, ProductCapabilities FROM Products WHERE Id = @Id",
                new { Id = id }
                ).ToList();
            return products.Count > 0 ? Ok(new ApiResponse<ProductResponseDTO> { 
                Success = true, 
                Message = "Product retrieved successfully.", 
                Data = products.FirstOrDefault() 
            }) : NotFound(new ApiResponse<object> { 
                Success = false, 
                Message = $"Product with ID {id} not found.", 
                Data = null 
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var connection = _db.CreateConnection();


            var rows = connection.Execute(
                "INSERT INTO Products (ProductId, ProductName, ProductPrice, ProductQuantity, ProductCapabilities) VALUES (@ProductId, @ProductName, @ProductPrice, @ProductQuantity, @ProductCapabilities)", product
                );

            return rows > 0 ? Ok("Product Added") : BadRequest("Product was not added");  

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var connection = _db.CreateConnection();

            var rows = connection.Execute(
                "UPDATE Products SET ProductName = @ProductName, ProductPrice = @ProductPrice, ProductQuantity = @ProductQuantity, ProductCapabilities = @ProductCapabilities WHERE Id = @Id",
                new { Id = id, ProductName = product.ProductName, ProductPrice = product.ProductPrice, ProductQuantity = product.ProductQuantity, ProductCapabilities = product.ProductCapabilities }
                );

            return rows > 0 ? Ok("Product Updated") : BadRequest("Product was not updated");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            using var connection = _db.CreateConnection();

            var rows = connection.Execute(
                "DELETE FROM Products WHERE Id = @Id",
                new { Id = id }
                );

            return rows > 0 ? Ok("Product Deleted") : BadRequest("Product was not deleted");
        }
    }
}
